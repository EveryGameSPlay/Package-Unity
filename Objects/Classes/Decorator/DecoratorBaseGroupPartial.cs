using System;
using System.Linq;

namespace Egsp.CSharp
{
    public abstract partial class DecoratorBase
    {
        protected ComponentGroup AddToGroup<TType>(TType component, Action<TType> groupAction)
            where TType : class, IComponent
        {
            var type = typeof(TType);
            var group = Groups.FirstOrDefault(x => x.ComponentsType == type) as ComponentGroup<TType>;

            // Создание новой группы.
            if (group == null)
            {
                group = new ComponentGroup<TType>(groupAction);
                Groups.Add(group);
            }

            group.AddComponent(component);
            return group;
        }
    }
}