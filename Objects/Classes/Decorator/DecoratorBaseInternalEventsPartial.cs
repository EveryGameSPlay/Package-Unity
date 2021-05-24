namespace Egsp.CSharp
{
    public abstract partial class DecoratorBase
    {
        protected virtual void OnDestroyInternal()
        {
        }

        protected virtual void OnAddComponentInternal(Component component)
        {
        }
    }
}