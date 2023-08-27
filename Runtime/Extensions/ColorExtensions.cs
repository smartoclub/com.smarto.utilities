namespace Smarto.Extensions
{
    using UnityEngine;

    public static class ColorExtensions
    {
        public static Color AsColor(this string color)
        {
            switch (color)
            {
                case "black":   return Color.black;
                case "blue":    return Color.blue;
                case "clear":   return Color.clear;
                case "cyan":    return Color.cyan;
                case "gray":    return Color.gray;
                case "green":   return Color.green;
                case "grey":    return Color.grey;
                case "magenta": return Color.magenta;
                case "red":     return Color.red;
                case "white":   return Color.white;
                case "yellow":  return Color.yellow;
                default:        return Color.white; // Default to white if the input color is not recognized
            }
        }
    }
}