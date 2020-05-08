using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuStateMachineSample_State_D : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Entered State D!");
        }

        private void OnDisable()
        {
            Debug.Log("Exited State D ..");
        }
    }
}