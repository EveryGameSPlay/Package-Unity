using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Egsp.Core
{
    public sealed class CriticalEventBus : EventBus
    {
       
        /// <summary>
        /// Собранные после вызова ошибки.
        /// </summary>
        [NotNull]
        public List<Exception> CollectedExceptions { get; private set; }
        
        public CriticalEventBus() : base()
        {
            CollectedExceptions = new List<Exception>();
        }

        public override void Raise<TSubscriberType>(Action<TSubscriberType> raiserAction)
        {
            CollectedExceptions.Clear();

            try
            {
                base.Raise(raiserAction);
            }
            catch (Exception ex)
            {
                CollectedExceptions.Add(ex);
            }
        }
    }
}