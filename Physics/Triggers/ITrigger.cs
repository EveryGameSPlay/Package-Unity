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
        void OnEnter(UnityAction<Trigg> triggAction);

        void OnExit(UnityAction<Trigg> triggAction);

        void Unsubscribe(UnityAction<Trigg> triggAction);
    }
}