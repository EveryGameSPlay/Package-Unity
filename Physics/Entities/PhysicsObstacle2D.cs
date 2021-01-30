using System;
using UnityEngine;

namespace Egsp.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsObstacle2D : MonoBehaviour, IMonoPhysicsEntity
    {
        private const float TimeToSleep = 0.4f;
        
        [SerializeField] protected bool dynamicOnForce = true;
        [SerializeField] protected bool kinematicOnInactive = true;
        [SerializeField] protected bool returnForce = false;
        [SerializeField] protected float returnForceMultiplier = 0.5f;
        
        protected Rigidbody2D Rig;
        protected float timeToSleep = TimeToSleep;

        public GameObject GameObject => gameObject;
        
        protected virtual void Awake()
        {
            Rig = GetComponent<Rigidbody2D>();
            Rig.isKinematic = true;
        }

        protected virtual void Update()
        {
            if (kinematicOnInactive)
                CheckSleep();
        }

        private void CheckSleep()
        {
            if (Rig.velocity == Vector2.zero)
            {
                timeToSleep -= Time.deltaTime;

                if (timeToSleep <= 0)
                    Rig.isKinematic = true;
            }
            else
            {
                timeToSleep = TimeToSleep;
            }
        }

        public virtual void ApplyForce(Force force, IPhysicsEntity actor = null)
        {
            if (dynamicOnForce)
            {
                Rig.isKinematic = false;
                timeToSleep = TimeToSleep;
                Rig.AddForce(force);
            }

            if(returnForce)
                if (actor != null)
                    actor.ApplyForce(-force * returnForceMultiplier);
        }
    }
}