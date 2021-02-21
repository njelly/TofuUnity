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
        public int Seed => _seed;
        public int Iteration => _iteration;
        
        private int _seed;
        private int _iteration;

        public TofuRandomState(int seed, int iteration = 0)
        {
            _seed = seed;
            _iteration = iteration;
        }

        public int Next()
        {
            _iteration++;
            return new Random(_seed + _iteration).Next();
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