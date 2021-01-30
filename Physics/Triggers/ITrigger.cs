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
        void EnterSubscribe(UnityAction<Trigg> triggAction);

        void ExitSubscribe(UnityAction<Trigg> triggAction);

        void Unsubscribe(UnityAction<Trigg> triggAction);
    }
}