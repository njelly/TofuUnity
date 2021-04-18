using System;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.DebugOverlay
{
    public class RandomLogSpammer : MonoBehaviour
    {
        public float interval;

        private float _timer;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0) 
                return;
            
            _timer += interval;

            var allLogTypes = new[] {LogType.Log, LogType.Warning, LogType.Error, LogType.Exception};
            var randomLogType = allLogTypes[UnityEngine.Random.Range(0, allLogTypes.Length)];
            switch (randomLogType)
            {
                case LogType.Assert:
                case LogType.Exception:
                    Debug.LogException(new Exception($"test {randomLogType.ToString()}"));
                    break;
                case LogType.Error:
                    Debug.LogError($"test {randomLogType.ToString()}");
                    break;
                case LogType.Log:
                    Debug.Log($"test {randomLogType.ToString()}");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"test {randomLogType.ToString()}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}