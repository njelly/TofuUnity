using FixMath.NET;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class FixVector2Extensions
    {
        public static Vector2 ToUnityVector2(this FixVector2 v) => new Vector2((float) v.x, (float) v.y);
        public static FixVector2 ToFixVector2(this Vector2 v) => new FixVector2((Fix64) v.x, (Fix64) v.y);
    }
}