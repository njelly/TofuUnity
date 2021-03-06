﻿using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class VectorExtensions
    {
        public static ECardinalDirection4 ToCardinalDirection4(this Vector2 v)
        {
            var angle = Mathf.Atan2(v.y, v.x);
            var quarter = Mathf.RoundToInt(Mathf.Round(4 * (angle / (Mathf.PI * 2f)) + 4) % 4);

            return (ECardinalDirection4)quarter;
        }

        public static ECardinalDirection8 ToCardinalDirection8(this Vector2 v)
        {
            var angle = Mathf.Atan2(v.y, v.x);
            var octant = Mathf.RoundToInt(Mathf.Round(8 * (angle / (Mathf.PI * 2f)) + 8) % 8);

            return (ECardinalDirection8)octant;
        }
        
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            var tx = v.x;
            var ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = sin * tx + cos * ty;
            return v;
        }
        
        public static bool HasLength(this Vector3 v) => 
            Mathf.Abs(v.x) > float.Epsilon || Mathf.Abs(v.y) > float.Epsilon || Mathf.Abs(v.z) > float.Epsilon;

        public static bool HasLength(this Vector2 v) =>
            Mathf.Abs(v.x) > float.Epsilon || Mathf.Abs(v.y) > float.Epsilon;
        
        public static Vector3Int RoundToVector3Int(this Vector3 v) => new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z)); 
        
        public static Vector2Int RoundToVector2Int(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

        public static bool IsApproximately(this Vector3 v, Vector3 other) => IsApproximately(v, other, float.Epsilon);
        public static bool IsApproximately(this Vector3 v, Vector3 other, float epsilon) => 
            v.x.IsApproximately(other.x, epsilon) && v.y.IsApproximately(other.y, epsilon) && v.z.IsApproximately(other.z, epsilon);
        
        public static bool IsApproximately(this Vector2 v, Vector2 other) => IsApproximately(v, other, float.Epsilon);
        public static bool IsApproximately(this Vector2 v, Vector2 other, float epsilon) =>
            v.x.IsApproximately(other.x, epsilon) && v.y.IsApproximately(other.y, epsilon);
    }
}