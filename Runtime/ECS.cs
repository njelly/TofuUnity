///////////////////////////////////////////////////////////////////////////////
//
//  TofuECS by Nathaniel
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;

namespace Tofunaut.TofuECS
{
    [Serializable]
    public class ECS
    {
        public Frame CurrentFrame { get; set; }
        private Blackboard _blackboard;

        public ECS(object config)
        {
            CurrentFrame = new Frame(config);
            _blackboard = new Blackboard();
        }

        public void Tick()
        {
            CurrentFrame.Tick();
            CurrentFrame.DequeueEvents(_blackboard);
            CurrentFrame = new Frame(CurrentFrame);
        }

        public void Subscribe<T>(Action<T> callback) where T : class, IBlackboardEvent
        {
            _blackboard.Subscribe(callback);
        }

        public void Unsubscribe<T>(Action<T> callback) where T : class, IBlackboardEvent
        {
            _blackboard.Unsubscribe(callback);
        }
    }

    public class OnEntityCreatedEvent : IBlackboardEvent
    {
        public readonly ulong entity;

        public OnEntityCreatedEvent(ulong entity)
        {
            this.entity = entity;
        }
    }

    public class OnEntityDestroyedEvent : IBlackboardEvent
    {
        public readonly ulong entity;

        public OnEntityDestroyedEvent(ulong entity)
        {
            this.entity = entity;
        }
    }

    public class OnComponentAddedEvent<T> : IBlackboardEvent where T : unmanaged
    {
        public readonly ulong entity;

        public OnComponentAddedEvent(ulong entity)
        {
            this.entity = entity;
        }
    }

    public unsafe class Frame
    {
        [Serializable]
        public class FrameState
        {
            public Dictionary<Type, object> typeToBag;
            public Dictionary<Type, (bool, object)> typeToFilters;
            public ulong entityCounter;
            public Dictionary<ulong, List<Type>> entityToComponentTypes;

            public FrameState Copy()
            {
                return new FrameState
                {
                    typeToBag = new Dictionary<Type, object>(typeToBag),
                    typeToFilters = new Dictionary<Type, (bool, object)>(typeToFilters),
                    entityCounter = entityCounter,
                    entityToComponentTypes = new Dictionary<ulong, List<Type>>(entityToComponentTypes),
                };
            }
        }
        
        private Dictionary<Type, object> _typeToBag;
        private Dictionary<Type[], (bool, object)> _typesToFilters;
        private ulong _entityCounter;
        private List<ECSSystem> _systems;
        private ulong _number;
        private Dictionary<ulong, List<Type>> _entityToComponentTypes;
        private readonly object _config;
        private Queue<IBlackboardEvent> _queuedEvents;
        
        public T Config<T>() => (T) _config;

        public ulong Number => _number;

        public Frame(object config)
        {
            _typeToBag = new Dictionary<Type, object>();
            _typesToFilters = new Dictionary<Type[], (bool, object)>();
            _systems = new List<ECSSystem>();
            _entityToComponentTypes = new Dictionary<ulong, List<Type>>();
            _number = 0;
            _config = config;
            _queuedEvents = new Queue<IBlackboardEvent>();
        }

        public Frame(Frame previous)
        {
            _typeToBag = previous._typeToBag;
            _typesToFilters = previous._typesToFilters;
            _systems = previous._systems;
            _entityCounter = previous._entityCounter;
            _entityToComponentTypes = previous._entityToComponentTypes;
            _number = previous._number + 1;
            _config = previous._config;
            _queuedEvents = new Queue<IBlackboardEvent>();
        }
        
        private Frame() { }

        public ulong Create()
        {
            _entityToComponentTypes.Add(++_entityCounter, new List<Type>());
            EnqueueEvent(new OnEntityCreatedEvent(_entityCounter));
            return _entityCounter;
        }

        public void Destroy(ulong entity)
        {
            if (!_entityToComponentTypes.TryGetValue(entity, out var typesList))
                return;

            foreach (var type in typesList)
            {
                var relevantKeys = _typesToFilters.Keys.Where(x => x.Contains(type));
                foreach (var relevantKey in relevantKeys)
                    if(_typeToBag.TryGetValue(type, out var ecbObj))
                    {
                        if (_typesToFilters.TryGetValue(relevantKey, out var isDirty))
                            isDirty.Item1 = true;
                        
                        var bag = (IEntityComponentBag) ecbObj;
                        bag.Free(entity);
                    }
            }
            
            EnqueueEvent(new OnEntityDestroyedEvent(entity));
        }

        public bool Exists(ulong entity)
        {
            return _entityToComponentTypes.ContainsKey(entity);
        }

        public void Add<T>(ulong entity) where T : unmanaged
        {
            if (!TryGetEntityComponentBag<T>(out var entityComponentBag))
                throw new Exception($"the type {nameof(T)} is not registered");

            var relevantKeys = _typesToFilters.Keys.Where(x => x.Contains(typeof(T)));
            foreach (var relevantKey in relevantKeys)
                if (_typesToFilters.TryGetValue(relevantKey, out var tuple))
                    tuple.Item1 = true;
            
            if (!_entityToComponentTypes.TryGetValue(entity, out var typesList))
                throw new Exception($"the entity {entity} does not exist");
            
            typesList.Add(typeof(T));

            entityComponentBag.Assign(entity);
            EnqueueEvent(new OnComponentAddedEvent<T>(entity));
        }

        public T* Get<T>(ulong entity) where T : unmanaged
        {
            if (!TryGetEntityComponentBag(out EntityComponentBag<T> entityComponentBag))
                throw new Exception($"the type {nameof(T)} is not registered");

            return entityComponentBag.Get(entity);
        }

        public bool TryGet<T>(ulong entity, out T* t) where T : unmanaged
        {
            if (!TryGetEntityComponentBag(out EntityComponentBag<T> entityComponentBag))
                throw new Exception($"the type {nameof(T)} is not registered");

            return entityComponentBag.TryGet(entity, out t);
        }

        public void Tick()
        {
            foreach (var system in _systems)
                system.Update(this);
        }

        public void RegisterComponent<T>(int size) where T : unmanaged
        {
            if (_typeToBag.ContainsValue(typeof(T)))
                throw new Exception($"the type {nameof(T)} is already registered");

            _typeToBag.Add(typeof(T), new EntityComponentBag<T>(size));
        }

        public void AddSystem(ECSSystem system) => _systems.Add(system);
        public void RemoveSystem(ECSSystem system) => _systems.Remove(system);

        public Filter<T> Filter<T>() where T : unmanaged
        {
            if (_typesToFilters.TryGetValue(new [] { typeof(T) }, out var pair) && pair.Item1)
            {
                var filter = (Filter<T>)pair.Item2;
                filter.Reset();
                return filter;
            }

            if (!_typeToBag.TryGetValue(typeof(T), out var ecbObj))
                throw new Exception($"the type {nameof(T)} is not registered");

            var ecb = (EntityComponentBag<T>)ecbObj;
            var cleanFilter = EntityComponentBag<T>.GetFilter(ecb);
            _typesToFilters[new [] { typeof(T) }] = (false, cleanFilter);
            return cleanFilter;
        }

        public Filter<T1, T2> Filter<T1, T2>() where T1 : unmanaged where T2 : unmanaged
        {
            if (_typesToFilters.TryGetValue(new [] { typeof(T1), typeof(T2) }, out var pair) && pair.Item1)
            {
                var filter = (Filter<T1, T2>)pair.Item2;
                filter.Reset();
                return filter;
            }

            if (!_typeToBag.TryGetValue(typeof(T1), out var ecbObj1))
                throw new Exception($"the type {nameof(T1)} is not registered");
            if (!_typeToBag.TryGetValue(typeof(T2), out var ecbObj2))
                throw new Exception($"the type {nameof(T2)} is not registered");

            var ecb1 = (EntityComponentBag<T1>)ecbObj1;
            var ecb2 = (EntityComponentBag<T2>)ecbObj2;
            var cleanFilter = EntityComponentBag<T1>.GetFilter(ecb1, ecb2);
            _typesToFilters[new [] { typeof(T1), typeof(T2) }] = (false, cleanFilter);
            return cleanFilter;
        }

        public Filter<T1, T2, T3> Filter<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
        {
            if (_typesToFilters.TryGetValue(new [] { typeof(T1), typeof(T2), typeof(T3) }, out var pair) && pair.Item1)
            {
                var filter = (Filter<T1, T2, T3>)pair.Item2;
                filter.Reset();
                return filter;
            }

            if (!_typeToBag.TryGetValue(typeof(T1), out var ecbObj1))
                throw new Exception($"the type {nameof(T1)} is not registered");
            if (!_typeToBag.TryGetValue(typeof(T2), out var ecbObj2))
                throw new Exception($"the type {nameof(T2)} is not registered");
            if (!_typeToBag.TryGetValue(typeof(T3), out var ecbObj3))
                throw new Exception($"the type {nameof(T3)} is not registered");

            var ecb1 = (EntityComponentBag<T1>)ecbObj1;
            var ecb2 = (EntityComponentBag<T2>)ecbObj2;
            var ecb3 = (EntityComponentBag<T3>)ecbObj3;
            var cleanFilter = EntityComponentBag<T1>.GetFilter(ecb1, ecb2, ecb3);
            _typesToFilters[new [] { typeof(T1), typeof(T2), typeof(T3) }] = (false, cleanFilter);
            return cleanFilter;
        }

        private bool TryGetEntityComponentBag<T>(out EntityComponentBag<T> entityComponentBag) where T : unmanaged
        {
            entityComponentBag = default;
            
            if (!_typeToBag.TryGetValue(typeof(T), out var ecb)) 
                return false;
            
            entityComponentBag = (EntityComponentBag<T>)ecb;
            return true;
        }

        public void EnqueueEvent(IBlackboardEvent blackboardEvent)
        {
            _queuedEvents.Enqueue(blackboardEvent);
        }

        public void DequeueEvents(Blackboard blackboard)
        {
            while (_queuedEvents.Count > 0)
            {
                var queuedEvent = _queuedEvents.Dequeue();
                blackboard.Invoke(queuedEvent);
            }
        }
    }

    public abstract class ECSSystem
    {
        public abstract void Update(Frame f);
    }

    public interface IEntityComponentBag
    {
        void Assign(ulong entity);
        void Free(ulong entity);
    }

    [Serializable]
    public unsafe class EntityComponentBag<T> : IEntityComponentBag where T : unmanaged
    {
        private ComponentBuffer<T> _componentBuffer;
        private (bool, ulong, int)[] _inUseToEntity;
        private int _nextFreeIndex;

        public EntityComponentBag(int size)
        {
            _componentBuffer = new ComponentBuffer<T>(size);
            _inUseToEntity = new (bool, ulong, int)[size];
            _nextFreeIndex = 0;

            for (var i = 0; i < _inUseToEntity.Length; i++)
                _inUseToEntity[i].Item3 = i;
        }

        public void Assign(ulong entity)
        {
            if (_nextFreeIndex >= _inUseToEntity.Length)
                throw new Exception("all components are in use");

            _inUseToEntity[_nextFreeIndex].Item1 = true;
            _inUseToEntity[_nextFreeIndex].Item2 = entity;
            _componentBuffer.Clear(_inUseToEntity[_nextFreeIndex].Item3);

            _nextFreeIndex++;
        }

        public void Free(ulong entity)
        {
            var i = 0;
            for (; i < _nextFreeIndex; i++)
                if (_inUseToEntity[i].Item2 == entity)
                    break;

            if (i >= _nextFreeIndex)
                return;

            _nextFreeIndex--;

            _inUseToEntity[i].Item1 = false;

            var temp = _inUseToEntity[i];
            _inUseToEntity[i] = _inUseToEntity[_nextFreeIndex];
            _inUseToEntity[_nextFreeIndex] = temp;
        }

        public T* Get(ulong entity)
        {
            var i = 0;
            for (; i < _nextFreeIndex; i++)
                if (_inUseToEntity[i].Item2 == entity)
                    return _componentBuffer.Get(_inUseToEntity[i].Item3);

            return null;
        }

        public bool TryGet(ulong entity, out T* t)
        {
            var i = 0;
            for (; i < _nextFreeIndex; i++)
                if (_inUseToEntity[i].Item2 == entity)
                    return _componentBuffer.TryGet(_inUseToEntity[i].Item3, out t);

            t = null;
            return false;
        }

        public static Filter<T1> GetFilter<T1>(EntityComponentBag<T1> bag) where T1 : unmanaged 
        {
            return new Filter<T1>(bag, bag._inUseToEntity.Select(x => x.Item2).ToArray());
        }

        public static Filter<T1, T2> GetFilter<T1, T2>(EntityComponentBag<T1> bag1, EntityComponentBag<T2> bag2) where T1 : unmanaged where T2 : unmanaged
        {
            var bag1Entities = bag1._inUseToEntity.Select(x => x.Item2);
            var bag2Entities = bag2._inUseToEntity.Select(x => x.Item2);
            var commonEntities = bag1Entities.Where(x => bag2Entities.Contains(x)).ToArray();
            return new Filter<T1, T2>(bag1, bag2, commonEntities);
        }

        public static Filter<T1, T2, T3> GetFilter<T1, T2, T3>(EntityComponentBag<T1> bag1, EntityComponentBag<T2> bag2, EntityComponentBag<T3> bag3) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
        {
            var bag1Entities = bag1._inUseToEntity.Select(x => x.Item2);
            var bag2Entities = bag2._inUseToEntity.Select(x => x.Item2);
            var bag3Entities = bag3._inUseToEntity.Select(x => x.Item2);
            var commonEntities = bag1Entities.Where(x => bag2Entities.Contains(x)).Where(x => bag3Entities.Contains(x)).ToArray();
            return new Filter<T1, T2, T3>(bag1, bag2, bag3, commonEntities);
        }
    }

    [Serializable]
    public unsafe class ComponentBuffer<T> where T : unmanaged
    {
        private T[] _components;

        public ComponentBuffer(int size)
        {
            _components = new T[size];
        }

        public T* Get(int index)
        {
            fixed (T* toReturn = &_components[index])
                return toReturn;
        }

        public bool TryGet(int index, out T* component)
        {
            component = null;
            if (index < 0 || index >= _components.Length)
                return false;

            fixed (T* toReturn = &_components[index])
            {
                component = toReturn;
                return true;
            }
        }

        public void Clear(int index)
        {
            _components[index] = default(T);
        }
    }

    public unsafe class Filter<T> where T : unmanaged
    {
        private readonly ulong[] _entitiesInUse;
        private readonly T*[] _components;
        private int _currentIndex;

        public Filter(EntityComponentBag<T> entityComponentBag, ulong[] entitiesInUse)
        {
            _entitiesInUse = entitiesInUse;
            _components = new T*[entitiesInUse.Length];
            for (var i = 0; i < entitiesInUse.Length; i++)
                _components[i] = entityComponentBag.Get(entitiesInUse[i]);
            _currentIndex = 0;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        public bool Next(out ulong entity, out T* component)
        {
            entity = default;
            component = default;

            if (_currentIndex >= _entitiesInUse.Length)
                return false;

            entity = _entitiesInUse[_currentIndex];
            component = _components[_currentIndex];
            
            _currentIndex++;

            return true;
        }
    }

    public unsafe class Filter<T1, T2> where T1 : unmanaged where T2 : unmanaged
    {
        private readonly ulong[] _entitiesInUse;
        private readonly T1*[] _components1;
        private readonly T2*[] _components2;
        private int _currentIndex;

        public Filter(EntityComponentBag<T1> entityComponentBag1, EntityComponentBag<T2> entityComponentBag2, ulong[] entitiesInUse)
        {
            _entitiesInUse = entitiesInUse;
            _components1 = new T1*[entitiesInUse.Length];
            _components2 = new T2*[entitiesInUse.Length];
            for (var i = 0; i < entitiesInUse.Length; i++)
            {
                _components1[i] = entityComponentBag1.Get(entitiesInUse[i]);
                _components2[i] = entityComponentBag2.Get(entitiesInUse[i]);
            }
            _currentIndex = 0;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        public bool Next(out ulong entity, out T1* component1, out T2* component2)
        {
            entity = default;
            component1 = default;
            component2 = default;

            if (_currentIndex >= _entitiesInUse.Length)
                return false;

            entity = _entitiesInUse[_currentIndex];
            component1 = _components1[_currentIndex];
            component2 = _components2[_currentIndex];
            
            _currentIndex++;

            return true;
        }
    }

    public unsafe class Filter<T1, T2, T3> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        private readonly ulong[] _entitiesInUse;
        private readonly T1*[] _components1;
        private readonly T2*[] _components2;
        private readonly T3*[] _components3;
        private int _currentIndex;

        public Filter(EntityComponentBag<T1> entityComponentBag1, EntityComponentBag<T2> entityComponentBag2, EntityComponentBag<T3> entityComponentBag3, ulong[] entitiesInUse)
        {
            _entitiesInUse = entitiesInUse;
            _components1 = new T1*[entitiesInUse.Length];
            _components2 = new T2*[entitiesInUse.Length];
            _components3 = new T3*[entitiesInUse.Length];
            for (var i = 0; i < entitiesInUse.Length; i++)
            {
                _components1[i] = entityComponentBag1.Get(entitiesInUse[i]);
                _components2[i] = entityComponentBag2.Get(entitiesInUse[i]);
                _components3[i] = entityComponentBag3.Get(entitiesInUse[i]);
            }
            _currentIndex = 0;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        public bool Next(out ulong entity, out T1* component1, out T2* component2, out T3* component3)
        {
            entity = default;
            component1 = default;
            component2 = default;
            component3 = default;

            if (_currentIndex >= _entitiesInUse.Length)
                return false;

            entity = _entitiesInUse[_currentIndex];
            component1 = _components1[_currentIndex];
            component2 = _components2[_currentIndex];
            component3 = _components3[_currentIndex];
            
            _currentIndex++;

            return true;
        }
    }
}
