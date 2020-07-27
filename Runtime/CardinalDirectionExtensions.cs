using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public enum ECardinalDirection4
    {
        East = 0,
        North = 1,
        West = 2,
        South = 3
    }

    public enum ECardinalDirection8
    {
        East = 0,
        NorthEast = 1,
        North = 2,
        NorthWest = 3,
        West = 4,
        SouthWest = 5,
        South = 6,
        SouthEast = 7
    }

    public static class CardinalDirectionExtensions
    {
        public static Vector2 ToVector2(this ECardinalDirection4 dir)
        {
            switch (dir)
            {
                default:
                case ECardinalDirection4.East:
                    return Vector2.right;
                case ECardinalDirection4.North:
                    return Vector2.up;
                case ECardinalDirection4.West:
                    return Vector2.left;
                case ECardinalDirection4.South:
                    return Vector2.down;
            }
        }

        public static Vector2Int ToVector2Int(this ECardinalDirection4 dir)
        {
            switch (dir)
            {
                default:
                case ECardinalDirection4.East:
                    return Vector2Int.right;
                case ECardinalDirection4.North:
                    return Vector2Int.up;
                case ECardinalDirection4.West:
                    return Vector2Int.left;
                case ECardinalDirection4.South:
                    return Vector2Int.down;
            }
        }
    }
}