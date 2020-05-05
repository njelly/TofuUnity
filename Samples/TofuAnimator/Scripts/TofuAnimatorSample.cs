using UnityEngine;
using UnityEngine.UI;
using System;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuAnimatorSample : MonoBehaviour
    {
        public Slider sliderInstance;
        public float animOffset = 1f;
        public float animTime = 3f;
        public int numSliders = 30;
        public TofuAnimator.EEaseType easeType;

        private void Start()
        {
            float offsetTime = animOffset;
            for (int i = 0; i < numSliders; i++)
            {
                Slider instance = Instantiate(sliderInstance, sliderInstance.transform.parent);

                float waitTime = offsetTime;
                new TofuAnimator.Sequence()
                    .Wait(waitTime)
                    .Then()
                    .Curve(easeType, animTime, (float percent) =>
                    {
                        instance.value = percent;
                    })
                    .Play();

                offsetTime += animOffset;
            }

            Destroy(sliderInstance.gameObject);
        }
    }
}