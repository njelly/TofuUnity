using Tofunaut.TofuUnity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.TofuUnity.Samples.TabManager
{
    [ExecuteInEditMode]
    public class FadeColorTab : Tab
    {
        public Image fadeImage;
        public Color openedColor;
        public Color closedColor;
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (fadeImage && !UnityEditor.EditorApplication.isPlaying)
                fadeImage.color = closedColor;
        }
        #endif

        protected override void OnOpen()
        {
            var startColor = fadeImage.color;
            fadeImage.gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, 0.5f, newValue =>
                {
                    fadeImage.color = Color.Lerp(startColor, openedColor, newValue);
                })
                .Play();
        }

        protected override void OnClose()
        {
            var startColor = fadeImage.color;
            fadeImage.gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, 0.5f, newValue =>
                {
                    fadeImage.color = Color.Lerp(startColor, closedColor, newValue);
                })
                .Play();
        }
    }
}