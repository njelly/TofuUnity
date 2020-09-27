using UnityEngine;

namespace Tofunaut.TofuUnity
{
    /// <summary>
    /// An implementation of a Singleton MonoBehviour.
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T: MonoBehaviour
    {

        public static bool HasInstance => _instance != null;

        protected static T _instance;

        protected virtual bool SetDontDestroyOnLoad => false;

        /// <summary>
        /// If true, destroy the gameObject when and instance already exists. If false, just destroy the component.
        /// </summary>
        protected virtual bool DestroyGameObjectWhenInstanceExists => false;

        protected virtual void Awake()
        {
            if(_instance != null)
            {
                Debug.LogErrorFormat("another instance of {0} already exists, this one will be destroyed", nameof(T));
                
                if(DestroyGameObjectWhenInstanceExists)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(this);
                }

                return;
            }

            _instance = this as T;

            if(SetDontDestroyOnLoad)
            {
                DontDestroyOnLoad(_instance);
            }
        }

        protected virtual void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }
    }
}