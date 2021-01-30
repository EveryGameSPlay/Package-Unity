using UnityEngine;

namespace Egsp.Extensions.Primitives
{
    public static class TransformExtensions
    {
        public static Vector3 DirectionTo(this Transform transform, Transform toTransform)
        {
            return (toTransform.position - transform.position).normalized;
        }
    }
}