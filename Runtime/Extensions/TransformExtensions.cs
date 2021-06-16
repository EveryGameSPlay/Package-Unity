using UnityEngine;

namespace Egsp.Core
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Возвращает вектор направления между начальной и конечной точками.
        /// </summary>
        public static Vector3 DirectionTo(this Transform transform, Transform toTransform)
        {
            return (toTransform.position - transform.position).normalized;
        }

        /// <summary>
        /// Возвращает истину, если позиция перед нами.
        /// </summary>
        public static bool InFront(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).z >= 0)
                return true;
            return false;
        }
        
        /// <summary>
        /// Возвращает истину, если позиция справа от нас.
        /// </summary>
        public static bool OnRight(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).x >= 0)
                return true;
            return false;
        }
        
        /// <summary>
        /// Возвращает истину, если позиция над нами.
        /// </summary>
        public static bool Above(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).y >= 0)
                return true;
            return false;
        }
    }
}