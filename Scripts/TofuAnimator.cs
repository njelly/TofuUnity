///////////////////////////////////////////////////////////////////////////////
//
//  Copyright (c) 2011 Bob Berkebile (pixelplacment)
//  PlEase direct any bugs/comments/suggestions to http://pixelplacement.com
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//
///////////////////////////////////////////////////////////////////////////////
//
//  TERMS OF USE - EASING EQUATIONS
//  Open source under the BSD License.
//  Copyright(c)2001 Robert Penner
//  All rights reserved.
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//  Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//  Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class TofuAnimator : MonoBehaviour
    {
        public enum EEaseType
        {
            Constant,
            EaseInQuad,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInSine,
            EaseOutSine,
            EaseInOutSine,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            Linear,
            Spring,
        }

        private delegate float EaseCallbackProxy(float start, float end, float value);
        public static CurveCallback EaseTypeToCallback(EEaseType easeType)
        {
            EaseCallbackProxy proxyCallback;
            switch (easeType)
            {
                case EEaseType.EaseInBack:
                    proxyCallback = EaseInBack;
                    break;
                case EEaseType.EaseInBounce:
                    proxyCallback = EaseInBounce;
                    break;
                case EEaseType.EaseInCirc:
                    proxyCallback = EaseInCirc;
                    break;
                case EEaseType.EaseInCubic:
                    proxyCallback = EaseInCubic;
                    break;
                case EEaseType.EaseInElastic:
                    proxyCallback = EaseInElastic;
                    break;
                case EEaseType.EaseInExpo:
                    proxyCallback = EaseInExpo;
                    break;
                case EEaseType.EaseInOutBack:
                    proxyCallback = EaseInOutBack;
                    break;
                case EEaseType.EaseInOutBounce:
                    proxyCallback = EaseInOutBounce;
                    break;
                case EEaseType.EaseInOutCirc:
                    proxyCallback = EaseInOutCirc;
                    break;
                case EEaseType.EaseInOutCubic:
                    proxyCallback = EaseInOutCubic;
                    break;
                case EEaseType.EaseInOutElastic:
                    proxyCallback = EaseInOutElastic;
                    break;
                case EEaseType.EaseInOutExpo:
                    proxyCallback = EaseInOutExpo;
                    break;
                case EEaseType.EaseInOutQuad:
                    proxyCallback = EaseInOutQuad;
                    break;
                case EEaseType.EaseInOutQuart:
                    proxyCallback = EaseInOutQuart;
                    break;
                case EEaseType.EaseInOutQuint:
                    proxyCallback = EaseInOutQuint;
                    break;
                case EEaseType.EaseInOutSine:
                    proxyCallback = EaseInOutSine;
                    break;
                case EEaseType.EaseInQuad:
                    proxyCallback = EaseInQuad;
                    break;
                case EEaseType.EaseInQuart:
                    proxyCallback = EaseInQuart;
                    break;
                case EEaseType.EaseInQuint:
                    proxyCallback = EaseInQuint;
                    break;
                case EEaseType.EaseInSine:
                    proxyCallback = EaseInSine;
                    break;
                case EEaseType.EaseOutBack:
                    proxyCallback = EaseOutBack;
                    break;
                case EEaseType.EaseOutBounce:
                    proxyCallback = EaseOutBounce;
                    break;
                case EEaseType.EaseOutCirc:
                    proxyCallback = EaseOutCirc;
                    break;
                case EEaseType.EaseOutCubic:
                    proxyCallback = EaseOutCubic;
                    break;
                case EEaseType.EaseOutElastic:
                    proxyCallback = EaseOutElastic;
                    break;
                case EEaseType.EaseOutExpo:
                    proxyCallback = EaseOutExpo;
                    break;
                case EEaseType.EaseOutQuad:
                    proxyCallback = EaseOutQuad;
                    break;
                case EEaseType.EaseOutQuart:
                    proxyCallback = EaseOutQuint;
                    break;
                case EEaseType.EaseOutSine:
                    proxyCallback = EaseOutSine;
                    break;
                case EEaseType.Linear:
                    proxyCallback = Linear;
                    break;
                case EEaseType.Spring:
                    proxyCallback = Spring;
                    break;
                case EEaseType.Constant:
                    proxyCallback = Constant;
                    break;
                case EEaseType.EaseOutQuint:
                    proxyCallback = EaseOutQuint;
                    break;
                default:
                    Debug.LogErrorFormat("TofuAnimator does not implement the ease type {0}", easeType.ToString());
                    proxyCallback = Constant;
                    break;
            }

            return (float v) => (proxyCallback(0, 1, v));
        }

        private static TofuAnimator _instance;

        public delegate void ValueCallback(float percent);
        public delegate float CurveCallback(float percent);


        private HashSet<Sequence> _activeSequences;
        private HashSet<Sequence> _toAdd = new HashSet<Sequence>();

        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("An instance of TofuAnimator already exists");
                Destroy(this);
                return;
            }

            _activeSequences = new HashSet<Sequence>();
            _toAdd = new HashSet<Sequence>();

            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            foreach (Sequence toAdd in _toAdd)
            {
                _activeSequences.Add(toAdd);
            }
            _toAdd.Clear();

            HashSet<Sequence> toRemove = new HashSet<Sequence>();
            foreach (Sequence activeSequence in _activeSequences)
            {
                activeSequence.Update(Time.deltaTime);
                if (activeSequence.AllComplete)
                {
                    toRemove.Add(activeSequence);
                }
            }

            foreach (Sequence completeSequence in toRemove)
            {
                _activeSequences.Remove(completeSequence);
            }
        }

        public static void Play(TofuAnimator.Sequence sequence)
        {
            if (!_instance)
            {
                _instance = new GameObject("TofuAnimator", new[] { typeof(TofuAnimator) }).GetComponent<TofuAnimator>();
            }

            _instance._toAdd.Add(sequence);
        }

        public class Sequence
        {
            public bool AllComplete => _clipSequence.Count == 0;

            private Queue<List<Clip>> _clipSequence;
            private List<Clip> _toEnqueue;

            public Sequence()
            {
                _clipSequence = new Queue<List<Clip>>();
                _toEnqueue = new List<Clip>();
            }

            public Sequence Curve(EEaseType easeType, float time, ValueCallback callback)
            {
                _toEnqueue.Add(new Clip(TofuAnimator.EaseTypeToCallback(easeType), time, callback));

                return this;
            }

            public Sequence Wait(float time)
            {
                float timer = 0f;

                return WaitUntil(() =>
                {
                    timer += Time.deltaTime;
                    return timer >= time;
                });
            }

            public Sequence WaitUntil(Func<bool> waitForCallback)
            {
                _toEnqueue.Add(new Clip((float percent) =>
                {
                    return waitForCallback.Invoke() ? 1f : 0f;
                },
                float.MaxValue, (float percent) => { }));

                return this;
            }

            public Sequence Execute(Action callback)
            {
                return WaitUntil(() =>
                {
                    callback?.Invoke();
                    return true;
                });
            }

            public Sequence Then()
            {
                _clipSequence.Enqueue(new List<Clip>(_toEnqueue));
                _toEnqueue.Clear();

                return this;
            }

            public void Play()
            {
                Then();
                TofuAnimator.Play(this);
            }

            public bool Update(float deltaTime)
            {
                if (AllComplete)
                {
                    return true;
                }

                List<Clip> currentStep = _clipSequence.Peek();
                bool isComplete = true;
                foreach (Clip clip in currentStep)
                {
                    if (!clip.Update(deltaTime))
                    {
                        isComplete = false;
                    }
                }

                if (isComplete)
                {
                    _clipSequence.Dequeue();
                }

                return isComplete;
            }
        }

        private class Clip
        {
            private readonly float _time;
            private readonly ValueCallback _callback;
            private readonly CurveCallback _curveCallback;

            private float _timer;
            private bool _complete;

            public Clip(CurveCallback curveCallback, float time, ValueCallback callback)
            {
                _callback = callback;
                _curveCallback = curveCallback;
                _time = time;

                _timer = 0f;
                _complete = false;
            }

            public bool Update(float deltaTime)
            {
                if (_complete)
                {
                    return true;
                }

                _timer += deltaTime;
                if (_timer >= _time)
                {
                    _timer = _time;
                }

                float percent = _curveCallback(_timer / _time);

                _callback(percent);

                _complete = Mathf.Abs(1f - percent) <= float.Epsilon;
                return _complete;
            }
        }

        #region Easing Curves

        private static float Constant(float start, float end, float value)
        {
            return end;
        }

        private static float Linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        private static float Clerp(float start, float end, float value)
        {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);
            float retval = 0.0f;
            float diff = 0.0f;
            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else
                retval = start + (end - start) * value;
            return retval;
        }

        private static float Spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        private static float EaseInQuad(float start, float end, float value)
        {
            end -= start;
            return end * value * value + start;
        }

        private static float EaseOutQuad(float start, float end, float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        private static float EaseInOutQuad(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end / 2 * value * value + start;
            value--;
            return -end / 2 * (value * (value - 2) - 1) + start;
        }

        private static float EaseInCubic(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value + start;
        }

        private static float EaseOutCubic(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        private static float EaseInOutCubic(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end / 2 * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value + 2) + start;
        }

        private static float EaseInQuart(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value + start;
        }

        private static float EaseOutQuart(float start, float end, float value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        private static float EaseInOutQuart(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end / 2 * value * value * value * value + start;
            value -= 2;
            return -end / 2 * (value * value * value * value - 2) + start;
        }

        private static float EaseInQuint(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        private static float EaseOutQuint(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        private static float EaseInOutQuint(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end / 2 * value * value * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value * value * value + 2) + start;
        }

        private static float EaseInSine(float start, float end, float value)
        {
            end -= start;
            return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
        }

        private static float EaseOutSine(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
        }

        private static float EaseInOutSine(float start, float end, float value)
        {
            end -= start;
            return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
        }

        private static float EaseInExpo(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
        }

        private static float EaseOutExpo(float start, float end, float value)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
        }

        private static float EaseInOutExpo(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        private static float EaseInCirc(float start, float end, float value)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        private static float EaseOutCirc(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        private static float EaseInOutCirc(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        private static float EaseInBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        private static float EaseOutBounce(float start, float end, float value)
        {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        private static float EaseInOutBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            if (value < d / 2)
                return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else
                return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        private static float EaseInBack(float start, float end, float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        private static float EaseOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value / 1) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        private static float EaseInOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        private static float EaseInElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        private static float EaseOutElastic(float start, float end, float value)
        {
            //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        private static float EaseInOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d / 2) == 2)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
                return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;

            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
    }

    #endregion
}
