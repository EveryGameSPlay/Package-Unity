using System;
using System.Collections.Generic;

namespace Egsp.CSharp
{
    /// <summary>
    /// Базовый интерфейс всех компонентов.
    /// </summary>
    public interface IComponent
    {
        ObjectLiveState LiveState { get; }
    }
    
    public abstract class Component : IComponent
    {
        public ObjectLiveState LiveState { get; private set; } = ObjectLiveState.Alive;
        
        /// <summary>
        /// Объект, который в данный момент несет данный компонент.
        /// Ссылка будет убрана только в конце уничтожения текущего компонента.
        /// </summary>
        public DecoratorBase Parent { get; private set; }

        public void SetParent(DecoratorBase parent)
        {
            if (Parent != null)
                throw new InvalidOperationException("Нельзя назначить родителя компоненту, который уже имеет родителя");
            
            Parent = parent;
        }

        private void RemoveParent()
        {
            if (Parent == null)
                return;

            Parent.Remove(this);
        }

        /// <summary>
        /// Уничтожает данный компонент.
        /// </summary>
        public void Destroy()
        {
            if (LiveState != ObjectLiveState.Alive)
                return;

            LiveState = ObjectLiveState.Destroying;
            
            DestroyInternal();
            
            RemoveParent();
            LiveState = ObjectLiveState.Destroyed;
        }

        protected virtual void DestroyInternal()
        {
        }
    }

    [Obsolete]
    public interface IInvokableComponent : IComponent
    {
        void Invoke();
    }
}