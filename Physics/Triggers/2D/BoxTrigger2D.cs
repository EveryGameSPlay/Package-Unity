using UnityEngine;

namespace Egsp.Core
{
    public class BoxTrigger2D : Trigger2D
    {
        [SerializeField] private float size = 1;

        public Vector2 Size => new Vector2(size, size);
        
        protected override void OnDrawGizmos()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawWireCube(Centre, new Vector3(size,size,0.2f));
        }

        protected override void Overlap()
        {
            Physics2D.OverlapBox(Centre, Size, 0, Filter2D, RuntimeResults);

            for (var i = 0; i < RuntimeResults.Length; i++)
            {
                var result = RuntimeResults[i];
                if(result == null)
                    continue;
                
                MarkObject(result.gameObject);
            }
        }
    }
}