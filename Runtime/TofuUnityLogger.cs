using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class TofuUnityLogger : MonoBehaviour, ITofuLogger
    {
        public void Info(string s)
        {
            Debug.Log(s);
        }

        public void Warn(string s)
        {
            Debug.LogWarning(s);
        }

        public void Error(string s)
        {
            Debug.LogError(s);
        }
    }
}