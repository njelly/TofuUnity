using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class TofuStateMachine : MonoBehaviour
    {
        private static Action _defaultTransition = () => { };

        // _states that should be registered when this GameObject is instantiated, don't modify in code
        // allows states to be set up in the inspector
        [SerializeField] protected List<MonoBehaviour> _states;

        private Dictionary<string, MonoBehaviour> _keyToState;
        private Dictionary<string, Dictionary<string, Action>> _fromToAction;
        private Dictionary<Type, MonoBehaviour> _typeToState;
        private string _currentState = string.Empty;

        protected virtual void Awake()
        {
            _keyToState = new Dictionary<string, MonoBehaviour>();
            _fromToAction = new Dictionary<string, Dictionary<string, Action>>();
            _typeToState = new Dictionary<Type, MonoBehaviour>();

            foreach (MonoBehaviour mb in _states)
            {
                RegisterState(mb);
            }
        }

        public void RegisterState<T>() where T : MonoBehaviour
        {
            MonoBehaviour mb = gameObject.GetComponentInChildren<T>();
            if (!mb)
            {
                mb = new GameObject(nameof(T), new Type[] { typeof(T) }).GetComponent<T>();
            }

            RegisterState(mb);
        }

        public void RegisterState(MonoBehaviour state)
        {
            if (_keyToState.ContainsKey(state.name))
            {
                Debug.LogError($"state machine already contains state {state.name}");
                return;
            }

            _keyToState.Add(state.name, state);
            _typeToState.Add(state.GetType(), state);

            state.gameObject.SetActive(false);
        }

        public void RegisterTransition<T1, T2>(Action onTransition = null)
        {
            if (!_typeToState.TryGetValue(typeof(T1), out MonoBehaviour fromState))
            {
                Debug.LogError($"the state {typeof(T1)} has not been registered");
            }
            if (!_typeToState.TryGetValue(typeof(T2), out MonoBehaviour toState))
            {
                Debug.LogError($"the state {typeof(T2)} has not been registered");
            }

            RegisterTransition(fromState.name, toState.name, onTransition);
        }

        public void RegisterTransition(string from, string to, Action onTransition = null)
        {
            if (!_fromToAction.TryGetValue(from, out Dictionary<string, Action> toAction))
            {
                toAction = new Dictionary<string, Action>();
                _fromToAction.Add(from, toAction);
            }

            toAction[to] = onTransition;
        }

        public void TransitionTo<T>() where T : MonoBehaviour
        {
            TransitionTo(_typeToState[typeof(T)].name);
        }

        public void TransitionTo(string key)
        {
            if (!string.IsNullOrEmpty(_currentState))
            {
                // disable the current state
                MonoBehaviour mb = _keyToState[_currentState];
                mb.gameObject.SetActive(false);

                // if the current state has been set, verify we've registered a transition
                if (_fromToAction.TryGetValue(_currentState, out Dictionary<string, Action> toAction))
                {
                    // invoke the transition callback while both states are disabled
                    toAction[key]?.Invoke();
                }
                else
                {
                    Debug.LogError($"the transition from {_currentState} to {key} has not been registered");
                    return;
                }
            }

            if (_keyToState.TryGetValue(key, out MonoBehaviour newState))
            {
                // enable the next state
                _currentState = newState.name;
                newState.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"the state {key} has not been registered, it cannot be transitioned to");
            }
        }
    }
}