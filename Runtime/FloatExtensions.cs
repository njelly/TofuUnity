using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class FloatExtensions
    {
        public static bool IsApproximately(this float v, float other) => v.IsApproximately(other, float.Epsilon);
        public static bool IsApproximately(this float v, float other, float epsilon) => Mathf.Abs(v - other) <= epsilon;
    }
}