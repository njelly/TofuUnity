using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuUnity
{
    
    public abstract class AppStateController<T1, T2> : SingletonBehaviour<T1> where T1 : MonoBehaviour where T2 : IAppStateModel
    {
        public static bool IsInstanceComplete => HasInstance && (_instance as AppStateController<T1, T2>).IsComplete;
        public static bool IsInstanceReady => HasInstance && (_instance as AppStateController<T1, T2>).IsReady;
        
        public bool IsComplete { get; private set; }
        public bool IsReady { get; protected set; }
        
        public T2 Model { get; private set; }

        public virtual void Complete()
        {
            IsComplete = true;
        }

        public virtual void SetModel(T2 model)
        {
            Model = model;
        }
    }
}