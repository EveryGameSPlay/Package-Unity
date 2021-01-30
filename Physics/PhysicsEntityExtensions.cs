using UnityEngine;

namespace Egsp.Core
{
    public static class PhysicsEntityExtensions
    {
        public static void ApplyForceFrom(this IMonoPhysicsEntity physicsEntity,
            Transform from, float power, ForceMode mode = ForceMode.VelocityChange)
        {
            physicsEntity.ApplyForce(
                new Force(physicsEntity.DirectionFrom(from) * power, mode));
        }
        
        public static void ApplyForceFrom(this IPhysicsEntity physicsEntity, Transform to,
            Transform from, float power, ForceMode mode = ForceMode.VelocityChange)
        {
            physicsEntity.ApplyForce(
                new Force((to.position-from.position).normalized * power, mode));
        }
        
        public static Vector3 DirectionFrom(this IMonoPhysicsEntity physicsEntity, Transform fromTransform)
        {
            return (physicsEntity.GameObject.transform.position - fromTransform.position).normalized;
        }
        
        public static bool IsMonoPhysicsEntity(this GameObject gameObject, out IMonoPhysicsEntity physicsEntity)
        {
            physicsEntity = gameObject.GetComponent<IMonoPhysicsEntity>();

            return physicsEntity != null;
        }
        
        public static bool IsPhysicsEntity(this GameObject gameObject, out IPhysicsEntity physicsEntity)
        {
            physicsEntity = gameObject.GetComponent<IPhysicsEntity>();

            return physicsEntity != null;
        }
        
        public static bool IsPhysicsEntity(this Collider2D collider2D, out IPhysicsEntity physicsEntity)
        {
            physicsEntity = collider2D.GetComponent<IPhysicsEntity>();

            return physicsEntity != null;
        }
    }
}