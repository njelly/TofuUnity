using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [CreateAssetMenu(fileName = "new TofuGameEvent", menuName = "Tofunaut/TofuGameEvent")]
    public sealed class TofuGameEvent : ScriptableObject
    {
        private List<TofuGameEventListener> listeners = new List<TofuGameEventListener>();

        public void Raise()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEventsRaised();
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