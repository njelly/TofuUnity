using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class Timeout : MonoBehaviour
    {
        public float timeLeft = 0f;

        private void Update()
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}