using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class VectorIntExtensions
    {
        public static int ManhattanDistance(this Vector2Int v)
        {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y);
        }
    }
}