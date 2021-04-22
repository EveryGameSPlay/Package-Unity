using System;
using UnityEngine;

namespace Egsp.Other
{
    /// <summary>
    /// Обертка для кривой редактора Unity.
    /// </summary>
    [Serializable]
    public class Curve
    {
        [SerializeField] private AnimationCurve curve;

        /// <summary>
        /// Получение значения по позиции точки на оси x.
        /// </summary>
        public float Get(float pointX)
        {
            if (curve == null)
                return 0;

            return curve.Evaluate(pointX);
        }
    }
}