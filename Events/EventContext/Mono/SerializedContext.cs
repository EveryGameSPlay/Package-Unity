using System;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Egsp.Core
{
    [Obsolete("Класс не готов к использованию.")]
    public abstract class SerializedContext : SerializedMonoBehaviour, IContext
    {
        public IEventBus Bus
        {
            get
            {
                if(_bus == null)
                    _bus = new EventBus();

                return _bus;
            }
        }
        private IEventBus _bus;

        public void AddEntity(IContextEntity contextEntity)
        {
            contextEntity.Context = this;
        }
    }
}