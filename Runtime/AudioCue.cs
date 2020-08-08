using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [CreateAssetMenu(fileName = "New AudioCue", menuName = "Tofunaut/AudioCue")]
    public class AudioCue : ScriptableObject
    {
        [Header("Clips")]
        [SerializeField] private List<AudioClip> _clips;

        [Header("Meta Data")]
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] private bool _loop;
        [SerializeField] private float _interval = -1;
        [SerializeField] private bool _randomize;
        [SerializeField] private bool _allowRandomRepeats;
        [SerializeField, Range(0f, 1f)] private float _spatialBlend;

        public void Play() => Play(null, () => { return true; });
        public void Play(Func<bool> condition) => Play(null, condition);
        public void Play(Transform source) => Play(source, () => { return true; });
        public void Play(Transform source, Func<bool> condition)
        {
            if (_clips.Count <= 0)
            {
                Debug.LogError($"no clips specified for AudioCue ({name})");
                return;
            }

            if (!condition.Invoke())
            {
                return;
            }

            bool destroySourceObjectWhenFinished = false;
            AudioSource audioSource;
            if (!source)
            {
                destroySourceObjectWhenFinished = true;
                GameObject go = new GameObject($"{this.name} AudioSource");
                audioSource = go.AddComponent<AudioSource>();
            }
            else
            {
                audioSource = source.gameObject.AddComponent<AudioSource>();
            }

            int clipIndex = 0;
            if (_randomize)
            {
                clipIndex = UnityEngine.Random.Range(0, _clips.Count);
            }

            Func<bool> impliedCondition = () =>
            {
                // it is implied we will play the sound as long as the audio source exists
                return audioSource && condition.Invoke();
            };

            PlayOnce(audioSource, clipIndex, impliedCondition, destroySourceObjectWhenFinished);
        }

        private void PlayOnce(AudioSource audioSource, int clipIndex, Func<bool> condition, bool destroySourceObjectWhenFinished)
        {
            audioSource.volume = _volume;
            audioSource.spatialBlend = _spatialBlend;

            audioSource.PlayOneShot(_clips[clipIndex]);

            float intervalToUse;
            if (_interval >= 0)
            {
                intervalToUse = _interval;
            }
            else
            {
                intervalToUse = _clips[clipIndex].length;
            }

            audioSource.gameObject.Sequence()
                .Wait(intervalToUse)
                .Then()
                .Execute(() =>
                {
                    int prevClipIndex = clipIndex;
                    clipIndex++;
                    bool conditionResult = condition.Invoke();
                    if (conditionResult && _loop)
                    {
                        if (_randomize)
                        {
                            List<int> availableIndexes = new List<int>();
                            for (int i = 0; i < _clips.Count; i++)
                            {
                                if (!_allowRandomRepeats && i == prevClipIndex)
                                {
                                    continue;
                                }

                                availableIndexes.Add(i);
                            }

                            clipIndex = availableIndexes[UnityEngine.Random.Range(0, availableIndexes.Count)];
                        }
                        else
                        {
                            clipIndex %= _clips.Count;
                        }

                        PlayOnce(audioSource, clipIndex, condition, destroySourceObjectWhenFinished);
                    }
                    else if (!conditionResult || clipIndex >= _clips.Count)
                    {
                        if (destroySourceObjectWhenFinished)
                        {
                            Destroy(audioSource.gameObject);
                        }
                        else
                        {
                            Destroy(audioSource);
                        }
                    }
                })
                .Play();
        }
    }
}