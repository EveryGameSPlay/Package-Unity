using Egsp.Core;
using UnityEngine;

namespace Egsp.Core
{
    public class EventContext : MonoBehaviour, IEventContext
    {
        public EventBus Bus
        {
            get
            {
                if(_bus == null)
                    _bus = new EventBus();

                return _bus;
            }
        }
        private EventBus _bus;

        public void SetupContextToEntities()
        {
            var entities = GetComponentsInChildren<IEventContextEntity>(true);

            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].SetEventBus(Bus);
            }
        }
    }
}