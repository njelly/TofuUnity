using System;
using UnityEngine;
using Random = System.Random;

namespace Tofunaut.TofuUnity
{
    /// <summary>
    /// Keeps track of the last value of System.Random so the sequence of the RNG can be resumed without having to
    /// regenerate the previous values.
    /// </summary>
    [Serializable]
    public class TofuRandomState
    {
        private int _seed;
        private int _lastValue;

        public TofuRandomState(int seed, int lastValue = 0)
        {
            _seed = seed;
            _lastValue = lastValue;
        }

        public int Next()
        {
            _lastValue = new Random(_seed + _lastValue).Next();
            return _lastValue;
        }

        public float NextFloat() => Range(0f, 1f);

        public float Range(float min, float max)
        {
            var n = Next();
            var ratio = (float)n / int.MaxValue;
            return (max - min) * ratio + min;
        }

        public int Range(int min, int max) => (int) Range((float) min, max);
    }
}