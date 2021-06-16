using UnityEngine;

namespace Egsp.Core
{
    public static class ColorExtensions
    {
        public static Color Random()
        {
            return new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
        }

        /// <summary>
        /// Возвращает случайный цвет из массива цветов.
        /// </summary>
        public static Color Random(this Color[] colors)
        {
            if(colors.Length == 0)
                return Color.gray;
            
            return colors[UnityEngine.Random.Range(0, colors.Length)];
        }
    }
}