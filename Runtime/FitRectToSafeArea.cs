using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [RequireComponent(typeof(RectTransform))]
    public class FitRectToSafeArea : MonoBehaviour
    {
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private ScreenOrientation _prevOrientation;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            _rectTransform = (RectTransform) transform;
            _prevOrientation = Screen.orientation;
            Resize();
        }

        private void Update()
        {
            if (_prevOrientation == Screen.orientation) 
                return;
        
            _prevOrientation = Screen.orientation;
            Resize();
        }
    
        private void Resize()
        {
            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            var pixelRect = _canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }
    }
}