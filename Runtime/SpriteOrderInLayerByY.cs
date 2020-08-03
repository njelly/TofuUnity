using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class SpriteOrderInLayerByY : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public bool invert;

        private void LateUpdate()
        {
            spriteRenderer.sortingOrder = Mathf.CeilToInt(transform.position.y);

            if (invert)
            {
                spriteRenderer.sortingOrder *= -1;
            }
        }
    }
}
