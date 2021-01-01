using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class ColorExtensions
    {
        public static Color WithRed(this Color c, float r) => new Color(r, c.g, c.b, c.a);
        public static Color WithGreen(this Color c, float g) => new Color(c.r, g, c.b, c.a);
        public static Color WithBlue(this Color c, float b) => new Color(c.r, c.g, b, c.a);
        public static Color WithAlpha(this Color c, float a) => new Color(c.r, c.g, c.b, a);
    }
}