using System;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class RichTextUtils
    {
        public static string Color(string original, Color color) => ColorAt(original, color, 0, original.Length);

        public static string ColorAt(string original, Color color, int startIndex) =>
            ColorAt(original, color, startIndex, int.MaxValue);

        public static string ColorAt(string original, Color color, int startIndex, int length)
        {
            startIndex = Mathf.Clamp(startIndex, 0, original.Length);
            if (startIndex >= original.Length)
                length = 0;

            length = Mathf.Clamp(length, 0, original.Length - startIndex);
            
            var coloredPart = original.Substring(startIndex, length);
            var firstPart = original.Substring(0, original.Length - (original.Length - startIndex));
            var secondPart = original.Substring(startIndex + length, original.Length - (startIndex + length));
            var colorCode = ColorUtility.ToHtmlStringRGBA(color);

            return $"{firstPart}<color=#{colorCode}>{coloredPart}</color>{secondPart}";
        }

        public static string Hide(string original) => Color(original, UnityEngine.Color.clear);
        
        public static string HideAt(string original, int startIndex) => 
            HideAt(original, startIndex, int.MaxValue);

        public static string HideAt(string original, int startIndex, int length) =>
            ColorAt(original, UnityEngine.Color.clear, startIndex, length);
    }
}