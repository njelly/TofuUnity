using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuAnimatorSample : MonoBehaviour
    {
        [RequireType(typeof(UnityEngine.Object))] public UnityEngine.Object test;
        
        public Slider sliderInstance;
        public float animOffset = 1f;
        public float animTime = 3f;
        public int numSliders = 30;
        public TofuAnimator.EEaseType easeType;
        public Text numSequencesLabel;

        List<Slider> _sliders = new List<Slider>();

        private void Start()
        {
            _sliders = new List<Slider>();

            float offsetTime = animOffset;
            for (int i = 0; i < numSliders; i++)
            {
                Slider instance = Instantiate(sliderInstance, sliderInstance.transform.parent);
                _sliders.Add(instance);

                float waitTime = offsetTime;

                instance.gameObject.Sequence()
                    .Wait(waitTime)
                    .Then()
                    .Curve(easeType, animTime, (float percent) =>
                    {
                        instance.value = percent;
                    })
                    .Play();

                offsetTime += animOffset;
            }

            sliderInstance.gameObject.SetActive(false);
        }

        private void Update()
        {
            numSequencesLabel.text = $"Number of Sequences: {TofuAnimator.NumSequencesPlaying}";
        }

        public void Replay()
        {
            float offsetTime = animOffset;
            foreach (Slider slider in _sliders)
            {
                float waitTime = offsetTime;
                slider.gameObject.Sequence()
                    .Wait(waitTime)
                    .Then()
                    .Curve(easeType, animTime, (float percent) =>
                    {
                        slider.value = percent;
                    })
                    .Play();

                offsetTime += animOffset;
            }
        }

        public void StopAll()
        {
            foreach (Slider slider in _sliders)
            {
                TofuAnimator.StopAll(slider.gameObject);
            }
        }
    }
}