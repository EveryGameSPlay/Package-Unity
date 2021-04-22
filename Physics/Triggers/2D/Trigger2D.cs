using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Базовый класс для всех триггеров в двумерном пространстве.
    /// </summary>
    public abstract class Trigger2D : TriggerBase
    {
        [SerializeField] private Vector2 offset;

        protected ContactFilter2D Filter2D = new ContactFilter2D();
        
        /// <summary>
        /// Используется для функций проверки объектов. Временный буффер найденных объектов.
        /// </summary>
        protected Collider2D[] RuntimeResults = new Collider2D[OverlapResults];
        
        protected Vector3 Centre => transform.position + (Vector3) offset;

        protected override void ClearRuntimeResults()
        {
            for (var i = 0; i < RuntimeResults.Length; i++)
                RuntimeResults[i] = null;
        }

        protected override Color GetGizmosColor() => GizmosColor;
    }
}