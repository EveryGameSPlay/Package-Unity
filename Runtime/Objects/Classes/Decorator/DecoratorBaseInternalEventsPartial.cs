namespace Egsp.CSharp
{
    public abstract partial class DecoratorBase
    {
        protected virtual void OnDestroyInternal()
        {
        }

        /// <summary>
        /// Вызывается после добавления компонента к декоратору.
        /// </summary>
        protected virtual void OnAddComponentInternal(IComponent component)
        {
        }
        
        /// <summary>
        /// Вызывается после добавления компонента к декоратору и после события OnAddComponentInternal.
        /// </summary>
        protected virtual void OnGroupComponent(IComponent component)
        {
            
        }
    }
}