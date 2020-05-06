using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuStateMachineSample_State_C : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Entered State C!");
        }

        private void OnDisabe()
        {
            Debug.Log("Exited State C ..");
        }
    }
}