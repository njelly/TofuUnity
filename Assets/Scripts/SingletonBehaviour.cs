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

        protected virtual void Awake()
        {
            if(_instance != null)
            {
                Debug.LogErrorFormat("another instance of {0} already exists, this one will be destroyed", nameof(T));
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