namespace Egsp.CSharp
{
    public abstract class Component
    {
        public DecoratorBase Parent { get; private set; }

        public static TComponent Create<TComponent>(DecoratorBase parent)
            where TComponent : Component, new()
        {
            var component = new TComponent();

            component.Parent = parent;
            return component;
        }

        public static void Attach(Component component, DecoratorBase parent)
        {
            if (component.Parent == null)
            {
                component.Parent = parent;
            }
        }

        public static void Detach(Component component, DecoratorBase parent)
        {
            if (parent == component.Parent)
            {
                component.Parent = null;
            }
        }
    }

    public interface IInvokableComponent
    {
        void Invoke();
    }
}