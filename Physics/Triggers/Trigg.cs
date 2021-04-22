using JetBrains.Annotations;
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Содержит информацию о воздействии объекта на триггер.
    /// </summary>
    public struct Trigg
    {
        /// <summary>
        /// Триггер, который был задействован.
        /// </summary>
        [NotNull] public ITrigger Trigger;
        
        /// <summary>
        /// Объект, который задейсвовал триггер.
        /// </summary>
        [NotNull] public GameObject Triggered;

        public Trigg([NotNull] ITrigger trigger,[NotNull] GameObject triggered)
        {
            Triggered = triggered;
            Trigger = trigger;
        }
    }
}