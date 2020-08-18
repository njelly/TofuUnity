using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class VectorIntExtensions
    {
        public static int ManhattanDistance(this Vector2Int v)
        {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y);
        }

        public static Vector2Int Rotate90Clockwise(this Vector2Int v)
        {
            return new Vector2Int(v.y, -v.x);
        }

        public static Vector2Int Flip180(this Vector2Int v)
        {
            return v.Rotate90Clockwise().Rotate90Clockwise();
        }

        public static Vector2Int Flip90CounterClockwise(this Vector2Int v)
        {
            return v.Rotate90Clockwise().Rotate90Clockwise().Rotate90Clockwise();
        }
    }
}