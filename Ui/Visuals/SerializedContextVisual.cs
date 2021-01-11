using System;
using Game;
using JetBrains.Annotations;

namespace Egsp.Core.Ui
{
    public abstract class SerializedContextVisual<TVisual> : SerializedVisual<TVisual>, IContextVisual<TVisual>
        where TVisual : class
    {
        [CanBeNull] protected EventBus ContextBus;
        
        public void SetEventBus(EventBus eventBus)
        {
            ContextBus = eventBus;
            
            ContextBus.Subscribe(this as TVisual ?? throw new NullReferenceException());
        }

        public virtual void AfterContextSetup()
        {
        }
    }
}