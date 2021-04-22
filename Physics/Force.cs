using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Представление физической силы, которое содержит вектор и режим применения.
    /// </summary>
    public struct Force
    {
        private static readonly Force zeroForce = new Force(Vector3.zero);

        /// <summary>
        /// Прикладываемая сила.
        /// </summary>
        public Vector3 vector;
        public ForceMode mode;

        public static Force Zero => zeroForce;

        public Force(Vector3 _vector, ForceMode _mode = ForceMode.Force)
        {
            vector = _vector;
            mode = _mode;
        }

        public static Force operator *(Force a, float f) => new Force(a.vector * f, a.mode);
        public static Force operator *(float f, Force a) => new Force(a.vector * f, a.mode);
        
        public static Force operator -(Force a) => new Force(-a.vector, a.mode);

    }
}