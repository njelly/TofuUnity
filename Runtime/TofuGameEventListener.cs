using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuUnity
{
    public sealed class TofuGameEventListener : MonoBehaviour
    {
        [SerializeField] private TofuGameEvent[] _gameEvents;

        public UnityEvent response;

        private void OnEnable()
        {
            foreach (TofuGameEvent gameEvent in _gameEvents)
            {
                gameEvent.RegisterListener(this);
            }
        }

        public void OnEventsRaised()
        {
            response.Invoke();
        }
    }
}
