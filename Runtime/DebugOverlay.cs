using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class DebugOverlay : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}