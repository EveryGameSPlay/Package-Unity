
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// <para>Компонент контекста. При старте ищет все сущности типа IContextEntity в своем и дочерних объектах.
    /// Также ищет все IContextEntityDetector.</para>
    ///
    /// <para>Чтобы использовать данный функионал нужно наследоваться от данного класса.</para>
    /// </summary>
    public abstract class Context : MonoBehaviour, IContext
    {
        
        private IEventBus _bus = new EventBus();

        private IEventBus _detectBus = new EventBus();

        public IEventBus Bus => _bus;

        protected virtual void Awake()
        {
            _detectBus.Subscribe<IContext>(this);
            
            FindAllEntities(this);
            FindAllDetectors(this);
        }

        protected void FindAllDetectors(Behaviour root)
        {
            var detectors = root.GetComponentsInChildren<IContextEntityDetector>();

            for (var i = 0; i < detectors.Length; i++)
            {
                AddDetector(detectors[i]);
            }
        }

        protected void AddDetector(IContextEntityDetector detector)
        {
            detector.DetectBus = _detectBus;
        }

        protected void FindAllEntities(Behaviour root)
        {
            var contextEntities = root.GetComponentsInChildren<IContextEntity>(true);

            for (var i = 0; i < contextEntities.Length; i++)
            {
                AddEntity(contextEntities[i]);
            }
        }

        public void AddEntity(IContextEntity contextEntity)
        {
            contextEntity.Context = this;
        }
    }
}