using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class Platform : MonoBehaviour
    {
        public MeshRenderer meshRenderer;

        private Material _mat;

        private void Start()
        {
            _mat = Instantiate(meshRenderer.materials[0]);
            meshRenderer.materials = new Material[] { _mat };
        }

        public void ChangeColor()
        {
            meshRenderer.material.color = Random.ColorHSV();
        }
    }
}