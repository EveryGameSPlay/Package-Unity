using System;
using UnityEngine.Events;

namespace Egsp.Core
{
    [Serializable]
    public class TriggerEvent : UnityEvent<Trigg>
    {
        
    }
    
    public interface ITrigger
    {
        void OnEnterSubscribe(UnityAction<Trigg> triggAction);

        void OnExitSubscribe(UnityAction<Trigg> triggAction);

        void Unsubscribe(UnityAction<Trigg> triggAction);
    }
}