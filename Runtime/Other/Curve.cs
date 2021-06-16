using System;
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Обертка для кривой редактора Unity.
    /// </summary>
    [Serializable]
    public struct Curve
    {
        [SerializeField] private AnimationCurve value;

        /// <summary>
        /// Получение значения по позиции точки на оси x.
        /// </summary>
        public float Get(float pointX)
        {
            if (value == null)
                return 0;

            return value.Evaluate(pointX);
        }
    }
}