using UnityEngine;
using ILogger = Tofunaut.TofuUnity.Interfaces.ILogger;

namespace Tofunaut.TofuUnity
{
    public class UnityLogger : ILogger
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