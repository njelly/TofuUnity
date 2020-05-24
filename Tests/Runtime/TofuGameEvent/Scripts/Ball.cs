using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class Ball : MonoBehaviour
    {
        private void Start()
        {
            this.gameObject.RequireComponent<Timeout>().timeLeft = 10f;
        }
    }
}