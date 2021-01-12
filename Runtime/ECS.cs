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

namespace Tofunaut.TofuECS
{
    [Serializable]
    public class ECS
    {
        public Frame CurrentFrame { get; private set; }

        public ECS()
        {
            CurrentFrame = new Frame();
        }

        public void Tick()
        {
            CurrentFrame.Tick();
            CurrentFrame = new Frame(CurrentFrame);
        }
    }

    [Serializable]
    public unsafe class Frame
    {
        private Dictionary<Type, object> _typeToBag;
        private Dictionary<Type, (bool, object)> _typeToFilters;
        private ulong _entityCounter;
        private List<ECSSystem> _systems;
        private ulong _number;
        private Dictionary<ulong, List<Type>> _entityToComponentTypes;

        public ulong Number => _number;

        public Frame()
        {
            _typeToBag = new Dictionary<Type, object>();
            _typeToFilters = new Dictionary<Type, (bool, object)>();
            _systems = new List<ECSSystem>();
            _entityToComponentTypes = new Dictionary<ulong, List<Type>>();
            _number = 0;
        }

        public Frame(Frame previous)
        {
            _typeToBag = previous._typeToBag;
            _typeToFilters = previous._typeToFilters;
            _systems = previous._systems;
            _entityCounter = previous._entityCounter;
            _entityToComponentTypes = previous._entityToComponentTypes;
            _number = previous._number + 1;
        }

        public ulong Create()
        {
            _entityToComponentTypes.Add(++_entityCounter, new List<Type>());
            return _entityCounter;
        }

        public void Destroy(ulong entity)
        {
            if (!_entityToComponentTypes.TryGetValue(entity, out var typesList))
                return;

            foreach (var type in typesList)
                if(_typeToBag.TryGetValue(type, out var ecbObj))
                {
                    if (_typeToFilters.TryGetValue(type, out var isDirty))
                        isDirty.Item1 = true;
                    
                    var bag = (IEntityComponentBag) ecbObj;
                    bag.Free(entity);
                }
        }

        public bool Exists(ulong entity)
        {
            return _entityToComponentTypes.ContainsKey(entity);
        }

        public void Add<T>(ulong entity) where T : unmanaged
        {
            if (!TryGetEntityComponentBag<T>(out var entityComponentBag))
                throw new Exception($"the type {nameof(T)} is not registered");

            if (_typeToFilters.TryGetValue(typeof(T), out var tuple))
                tuple.Item1 = true;
            
            if (!_entityToComponentTypes.TryGetValue(entity, out var typesList))
                throw new Exception($"the entity {entity} does not exist");
            
            typesList.Add(typeof(T));

            entityComponentBag.Assign(entity);
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
            if (_typeToFilters.TryGetValue(typeof(T), out var pair) && pair.Item1)
            {
                var filter = (Filter<T>)pair.Item2;
                filter.Reset();
                return filter;
            }

            if (!_typeToBag.TryGetValue(typeof(T), out var ecbObj))
                throw new Exception($"the type {nameof(T)} is not registered");

            var ecb = (EntityComponentBag<T>)ecbObj;
            var cleanFilter = ecb.GetFilter();
            _typeToFilters[typeof(T)] = (false, cleanFilter);
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

        public Filter<T> GetFilter()
        {
            var onlyInUse = new (bool, ulong, int)[_nextFreeIndex];
            Array.Copy(_inUseToEntity, onlyInUse, _nextFreeIndex);
            return new Filter<T>(this, onlyInUse.Select(x => x.Item2).ToArray());
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
        private readonly EntityComponentBag<T> _entityComponentBag;
        private readonly ulong[] _onlyInUse;
        private int _currentIndex;

        public Filter(EntityComponentBag<T> entityComponentBag, ulong[] onlyInUse)
        {
            _entityComponentBag = entityComponentBag;
            _onlyInUse = onlyInUse;
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

            if (_currentIndex >= _onlyInUse.Length)
                return false;

            entity = _onlyInUse[_currentIndex];
            component = _entityComponentBag.Get(entity);
            
            _currentIndex++;

            return true;
        }
    }
}
