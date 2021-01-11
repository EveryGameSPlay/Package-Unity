using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Egsp.Core
{
    public class SerializedEventContext : SerializedMonoBehaviour, IEventContext
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

        [OdinSerialize] private List<IEventContextEntity> _entities;

        public void SetupContextToEntities()
        {
            var entities = GetComponentsInChildren<IEventContextEntity>(true);

            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].SetEventBus(Bus);
            }

            _entities = entities.ToList();
        }
    }
}