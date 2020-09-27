using UnityEngine;

namespace Tofunaut.TofuUnity
{
    /// <summary>
    /// An implementation of a Singleton MonoBehviour.
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T: MonoBehaviour
    {
        /// <summary>
        /// True if an instance of this type already exists.
        /// </summary>
        public static bool HasInstance => _instance != null;

        protected static T _instance;

        /// <summary>
        /// Set the insance as DontDestroyOnLoad.
        /// </summary>
        protected virtual bool SetDontDestroyOnLoad => false;

        /// <summary>
        /// If true, an error message will not be printed when another instance exists (sometimes second instances may be expected).
        /// </summary>
        protected virtual bool SuppressError => false;

        /// <summary>
        /// If true, destroy the gameObject when and instance already exists. If false, just destroy the component.
        /// </summary>
        protected virtual bool DestroyGameObjectWhenInstanceExists => false;

        protected virtual void Awake()
        {
            if(HasInstance)
            {
                if(!SuppressError)
                {
                    Debug.LogErrorFormat($"another instance of {typeof(T).FullName} already exists, this one will be destroyed");
                }
                
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