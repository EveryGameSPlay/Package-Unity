using UnityEngine;

namespace Egsp.Core
{
    public static class QuaternionExtensions
    {
        public static Quaternion LookRotation2D(this Vector3 direction)
        {
            return Quaternion.LookRotation(Vector3.forward, -direction);
        }
        
        public static Quaternion LookRotation2D(this Vector2 direction)
        {
            return Quaternion.LookRotation(Vector3.forward, -direction);
        }
    }
}