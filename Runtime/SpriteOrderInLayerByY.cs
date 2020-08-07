using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [ExecuteInEditMode]
    public class SpriteOrderInLayerByY : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public bool invert;

        private void LateUpdate()
        {
            if (!spriteRenderer)
            {
                return;
            }

            spriteRenderer.sortingOrder = Mathf.CeilToInt(transform.position.y);
            if (invert)
            {
                spriteRenderer.sortingOrder *= -1;
            }
        }
    }
}
