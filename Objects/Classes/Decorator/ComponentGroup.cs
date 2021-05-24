using System;
using System.Collections.Generic;

namespace Egsp.CSharp
{
    /// <summary>
    /// Группа компонентов, к которой можно применить одинаковое действие.
    /// </summary>
    public abstract class ComponentGroup
    {
        public readonly Type ComponentsType;

        protected ComponentGroup(Type componentsType)
        {
            ComponentsType = componentsType;
        }
    }

    /// <summary>
    /// Группа компонентов, к которой можно применить одинаковое действие.
    /// </summary>
    public class ComponentGroup<TComponentInvokeType> : ComponentGroup
        where TComponentInvokeType : class, IComponent
    {
        /// <summary>
        /// Все текущие компоненты группы.
        /// Уничтоженные компоненты очищаются после каждого вызова Invoke.
        /// </summary>
        public readonly List<TComponentInvokeType> Components;

        /// <summary>
        /// Действие, вызываемое для каждого компонента.
        /// </summary>
        public readonly Action<TComponentInvokeType> Action;

        public ComponentGroup(Action<TComponentInvokeType> action) : base(typeof(TComponentInvokeType))
        {
            Action = action;
            Components = new List<TComponentInvokeType>();
        }

        public void AddComponent(TComponentInvokeType component)
        {
            Components.Add(component);
        }

        /// <summary>
        /// Вызывает заданное действие для всех компонентов в рабочем состоянии.
        /// </summary>
        public void Invoke()
        {
            for (var i = 0; i < Components.Count; i++)
            {
                var component = Components[i];

                // If not alive.
                if (component == null ||
                    component.LiveState != ObjectLiveState.Alive)
                {
                    Components[i] = null;
                    continue;
                }

                Action?.Invoke(component);

                ClearNulls();
            }
        }

        private void ClearNulls()
        {
            Components.RemoveAll(x => x == null);
        }
    }
}