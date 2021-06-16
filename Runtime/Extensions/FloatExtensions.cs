
using System.Globalization;

namespace Egsp.Core
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Самый эффективный способ округления числа и перевода его в строку
        /// </summary>
        /// <param name="f">Число</param>
        /// <param name="digits">Количество знаков после запятой</param>
        /// <returns></returns>
        public static string ToString(this float f, int digits)
        {
            return System.Math.Round(f, digits).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Возвращает число относительно промежутка (min--max) (от нуля до единицы)
        /// </summary>
        public static float ToNormalized(this float f, float min, float max)
        {
            if (min >= max)
                return 0;

            var normalized = (f - min) / (max - min);

            return normalized;
        }

        public static float ToNormalized(this int i, float min, float max)
        {
            if (min >= max)
                return 0;

            var normalized = (i - min) / (max - min);

            return normalized;
        }
    }
}