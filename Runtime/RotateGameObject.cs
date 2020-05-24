using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class RotateGameObject : MonoBehaviour
    {
        public Vector3 axisVelocities;

        private void Update()
        {
            transform.localRotation *= Quaternion.Euler(axisVelocities * Time.deltaTime);
        }
    }
}