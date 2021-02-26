using UnityEngine;

namespace Egsp.Core
{
    public readonly struct MinMaxFloat
    {
        public readonly float Min;
        public readonly float Max;

        public float Random => UnityEngine.Random.Range(Min, Max);

        public MinMaxFloat(float min, float max)
        {
            Min = Mathf.Abs(min);
            Max = Mathf.Abs(max);

            if ((Max - Min) < float.Epsilon)
                Max += 1f;
        }
    }
}