using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class VectorExtensions
    {
        public static ECardinalDirection4 ToCardinalDirection4(this Vector2 v)
        {
            float angle = Mathf.Atan2(v.y, v.x);
            int quarter = Mathf.RoundToInt(Mathf.Round(4 * (angle / (Mathf.PI * 2f)) + 4) % 4);

            return (ECardinalDirection4)quarter;
        }

        public static ECardinalDirection8 ToCardinalDirection8(this Vector2 v)
        {
            float angle = Mathf.Atan2(v.y, v.x);
            int octant = Mathf.RoundToInt(Mathf.Round(8 * (angle / (Mathf.PI * 2f)) + 8) % 8);

            return (ECardinalDirection8)octant;
        }
    }
}