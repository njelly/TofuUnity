using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuStateMachineSample_State_B : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Entered State B!");
        }

        private void OnDisabe()
        {
            Debug.Log("Exited State B ..");
        }
    }
}