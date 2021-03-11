using UnityEngine;

namespace Egsp.Extensions.Primitives
{
    public static class TransformExtensions
    {
        public static Vector3 DirectionTo(this Transform transform, Transform toTransform)
        {
            return (toTransform.position - transform.position).normalized;
        }

        public static bool InFront(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).z >= 0)
                return true;
            return false;
        }
        
        public static bool OnRight(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).x >= 0)
                return true;
            return false;
        }
        
        /// <summary>
        /// Position is upper.
        /// </summary>
        public static bool Above(this Transform transform, Vector3 position)
        {
            if (transform.InverseTransformPoint(position).y >= 0)
                return true;
            return false;
        }
    }
}