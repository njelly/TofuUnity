using System;
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
        [Space(10)]
        [Tooltip("rule of thumb: 0 for music, 1 for SFX, no common reason for in-between values")]
        [SerializeField, Range(0f, 1f)] private float _spatialBlend;
        [Tooltip("keep close to gameObject but increase if the sound is too quiet on screen")]
        [SerializeField] private float _minSpatialRadius = 1;
        [Tooltip("beyond this will be muted, keep in mind camera viewport and what the player should hear")]
        [SerializeField] private float _maxSpatialRadius = 100;
        // TODO: Unity does not have a Debug.DrawSphere()
        //[Header("Debug")]
        //[SerializeField] private bool _showDebug;
        //[SerializeField] private Color _spatialRadiusColor = Color.green;

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
            audioSource.minDistance = _minSpatialRadius;
            audioSource.maxDistance = _maxSpatialRadius;
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