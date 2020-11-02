using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity.Interfaces;

namespace Tofunaut.TofuUnity
{
    public class Blackboard
    {
        private HashSet<Tuple<Type, int, Action<object>>> _toAdd;
        private HashSet<Tuple<Type, int>> _toRemove;
        private Dictionary<Type, List<Tuple<int, Action<object>>>> _typeToCallbacks;

        public Blackboard()
        {
            _toAdd = new HashSet<Tuple<Type, int, Action<object>>>();
            _toRemove = new HashSet<Tuple<Type, int>>();
            _typeToCallbacks = new Dictionary<Type, List<Tuple<int, Action<object>>>>();
        }

        public void Subscribe<T>(Action<T> callback) where T : class, IBlackboardEvent
        {
            _toAdd.Add(new Tuple<Type, int, Action<object>>(typeof(T), callback.GetHashCode(), obj =>
            {
                callback.Invoke((T) obj);
            }));
        }

        public void Unsubscribe<T>(Action<T> callback) where T : class, IBlackboardEvent
        {
            _toRemove.Add(new Tuple<Type, int>(typeof(T), callback.GetHashCode()));
        }

        public void Invoke<T>(T data) where T : class, IBlackboardEvent
        {
            // first add any queued up callbacks
            foreach (var (type, hashCode, action) in _toAdd)
            {
                if (!_typeToCallbacks.TryGetValue(type, out var callbackListToAdd))
                {
                    callbackListToAdd = new List<Tuple<int, Action<object>>>();
                    _typeToCallbacks.Add(type, callbackListToAdd);
                }
                callbackListToAdd.Add(new Tuple<int, Action<object>>(hashCode, action));
            }
            _toAdd.Clear();

            // remove callbacks
            foreach (var (type, hashcode) in _toRemove)
                if (_typeToCallbacks.TryGetValue(type, out var callbackListToRemove))
                    callbackListToRemove.RemoveAll(x => x.Item1 == hashcode);
            _toRemove.Clear();

            // invoke the callbacks for this type
            if (!_typeToCallbacks.TryGetValue(typeof(T), out var callbackListToInvoke))
                return;
            foreach (var (_, action) in callbackListToInvoke)
                action.Invoke(data);
        }
    }
}