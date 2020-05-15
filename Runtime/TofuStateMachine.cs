using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class TofuStateMachine : MonoBehaviour
    {
        private static Action _DefaultTransition = () => { };

        // _states that should be registered when this GameObject is instantiated, don't modify in code
        // allows states to be set up in the inspector
        [SerializeField] protected List<MonoBehaviour> _states;

        public string CurrentState => _currentState;

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
                mb.transform.SetParent(transform);
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

            // register transition from null state to this state by default with the no-op transition
            RegisterTransition(string.Empty, state.name, _DefaultTransition);
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
            if (string.IsNullOrEmpty(to))
            {
                Debug.LogError("cannot transition to a null state");
                return;
            }

            if (!_fromToAction.TryGetValue(from, out Dictionary<string, Action> toAction))
            {
                toAction = new Dictionary<string, Action>();
                _fromToAction.Add(from, toAction);
            }

            toAction[to] = onTransition;
        }

        public T TransitionTo<T>() where T : MonoBehaviour
        {
            return TransitionTo(_typeToState[typeof(T)].name) as T;
        }

        public MonoBehaviour TransitionTo(string key)
        {
            MonoBehaviour currentStateBehaviour = null;
            if (!string.IsNullOrEmpty(_currentState))
            {
                // disable the current state
                currentStateBehaviour = _keyToState[_currentState];
                currentStateBehaviour.gameObject.SetActive(false);
            }

            // do this check outside of the isNullOrEmpty check to allow for transitions from null state to a new state
            // if the current state has been set, verify we've registered a transition
            if (_fromToAction.TryGetValue(_currentState, out Dictionary<string, Action> toAction))
            {
                // invoke the transition callback while both states are disabled
                toAction[key]?.Invoke();
            }

            if (_keyToState.TryGetValue(key, out currentStateBehaviour))
            {
                // enable the next state
                _currentState = currentStateBehaviour.name;
                currentStateBehaviour.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"the state {key} has not been registered, it cannot be transitioned to");
            }

            return currentStateBehaviour;
        }
    }
}