using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuStateMachineSample_State_A : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Entered State A!");
        }

        private void OnDisable()
        {
            Debug.Log("Exited State A...");
        }
    }
}