using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [CreateAssetMenu(fileName = "new TofuGameEvent", menuName = "Tofunaut/TofuGameEvent")]
    public sealed class TofuGameEvent : ScriptableObject
    {
        private List<TofuGameEventListener> listeners;

        private void OnEnable()
        {
            listeners = new List<TofuGameEventListener>();
        }

        public void Raise()
        {
            foreach (TofuGameEventListener listener in listeners)
            {
                listener.OnEventsRaised();
            }
        }

        public void RegisterListener(TofuGameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(TofuGameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}