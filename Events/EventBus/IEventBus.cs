using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Egsp.Core
{
    public interface IEventBus
    {
        [NotNull]
        Dictionary<Type, List<WeakReference>> Subscribers { get; }

        void Subscribe<TSubscriberType>([NotNull] TSubscriberType subscriber)
            where TSubscriberType : class;

        void Raise<TSubscriberType>([NotNull] Action<TSubscriberType> raiserAction)
            where TSubscriberType : class;
    }
}