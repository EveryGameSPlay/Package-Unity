using UnityEngine;

namespace Egsp.Core
{
    public class CircleTrigger2D : Trigger2D
    {
        [SerializeField] private float size = 1;

        public float Size => size;
        
        protected override void Overlap()
        {
            Physics2D.OverlapCircle(Centre, Size, Filter2D, RuntimeResults);

            for (var i = 0; i < RuntimeResults.Length; i++)
            {
                var result = RuntimeResults[i];
                if(result == null)
                    continue;
                
                MarkObject(result.gameObject);
            }
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = GetGizmosColor();
            Gizmos.DrawWireSphere(Centre, Size);
        }
    }
}