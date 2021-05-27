using System;
using System.Linq;
using Egsp.Core;

namespace Egsp.CSharp
{
    public abstract partial class DecoratorBase
    {
        /// <summary>
        /// <para>Добавляет объект к существующей группе (создает новую при отсутствии).</para>
        /// <para>Возвращает подходящую группу. Если возвращено None, значит объект не подходит по типу.
        /// Следует сохранять ссылку на полученную группу, чтобы позже ее использовать.</para>
        /// <para>Для одних и тех же типов группа будет возвращена одна и та же.</para>
        /// </summary>
        protected Option<ComponentGroup<TInterfaceType>> Group<TInterfaceType>(IComponent component)
            where TInterfaceType : class, IComponent
        {
            if (component is TInterfaceType castedComponent)
            {
                var type = typeof(TInterfaceType);
                var group = Groups.FirstOrDefault(x => x.ComponentsType == type) as ComponentGroup<TInterfaceType>;

                // Создание новой группы.
                if (group == null)
                {
                    group = new ComponentGroup<TInterfaceType>();
                    Groups.Add(group);
                }

                group.AddComponent(castedComponent);
                return group;
            }

            return Option<ComponentGroup<TInterfaceType>>.None;
        }
    }
}