using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class Ball : MonoBehaviour
    {
        public TofuGameEvent bounceEvent;

        private void Start()
        {
            this.gameObject.RequireComponent<Timeout>().timeLeft = 10f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("raise");
            bounceEvent.Raise();
        }
    }
}