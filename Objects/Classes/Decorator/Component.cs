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

        DecoratorBase Decorator { get; }

        void SetDecorator(DecoratorBase parent);

        void Destroy();
    }
    
    public abstract class Component : IComponent
    {
        public ObjectLiveState LiveState { get; private set; } = ObjectLiveState.Alive;
        
        /// <summary>
        /// Объект, который в данный момент несет данный компонент.
        /// Ссылка будет убрана только в конце уничтожения текущего компонента.
        /// </summary>
        public DecoratorBase Decorator { get; private set; }

        public void SetDecorator(DecoratorBase parent)
        {
            if (Decorator != null)
                throw new InvalidOperationException("Нельзя назначить декоратор компоненту," +
                                                    " который уже имеет декоратор");
            
            Decorator = parent;
        }

        private void RemoveDecorator()
        {
            if (Decorator == null)
                return;

            Decorator.Remove(this);
        }

        /// <summary>
        /// Уничтожает данный компонент.
        /// </summary>
        public void Destroy()
        {
            if (LiveState != ObjectLiveState.Alive)
                return;

            LiveState = ObjectLiveState.Destroying;
            
            OnDestroyInternal();
            
            RemoveDecorator();
            LiveState = ObjectLiveState.Destroyed;
        }

        /// <summary>
        /// Вызывается перед окончательным уничтожением.
        /// </summary>
        protected virtual void OnDestroyInternal()
        {
        }
    }

    [Obsolete]
    public interface IInvokableComponent : IComponent
    {
        void Invoke();
    }
}