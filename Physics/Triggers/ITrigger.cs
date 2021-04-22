using System;
using UnityEngine.Events;

namespace Egsp.Core
{
    [Serializable]
    public class TriggerEvent : UnityEvent<Trigg>
    {
        
    }
    
    /// <summary>
    /// Объекты данного типа засекают физические объекты и вызывают подписанные события.
    /// </summary>
    public interface ITrigger
    {
        void OnEnterSubscribe(UnityAction<Trigg> triggAction);

        void OnExitSubscribe(UnityAction<Trigg> triggAction);

        void Unsubscribe(UnityAction<Trigg> triggAction);
    }
}