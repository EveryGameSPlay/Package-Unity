using UnityEngine;

namespace Egsp.Core
{
    public readonly struct MinMaxInt
    {
        public readonly int Min;
        public readonly int Max;

        public int Random => UnityEngine.Random.Range(Min, Max);

        public MinMaxInt(int min, int max)
        {
            Min = (int)Mathf.Abs(min);
            Max = (int)Mathf.Abs(max);

            if ((Max - Min) < float.Epsilon)
            {
                Min = int.MinValue;
                Max = int.MaxValue;
            }
        }

        public float Normalize(int value)
        {
            if (value >= Max)
                return 1;

            if (value <= Min)
                return 0;

            return value.ToNormalized(Min, Max);
        }
    }
}