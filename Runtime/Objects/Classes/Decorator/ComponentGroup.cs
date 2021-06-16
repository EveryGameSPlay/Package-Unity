using System;
using System.Collections.Generic;

namespace Egsp.Core.Components
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

        public ComponentGroup() : base(typeof(TComponentInvokeType))
        {
            Components = new List<TComponentInvokeType>();
        }

        public void AddComponent(TComponentInvokeType component)
        {
            Components.Add(component);
        }

        /// <summary>
        /// Вызывает заданное действие для всех компонентов в рабочем состоянии.
        /// </summary>
        public void Invoke(Action<TComponentInvokeType> action)
        {
            for (var i = 0; i < Components.Count; i++)
            {
                var component = Components[i];

                if(CheckAlive(component,i) != ObjectLiveState.Alive)
                    continue;

                action?.Invoke(component);

                ClearNulls();
            }
        }

        /// <summary>
        /// Метод для ручного управления вызовами. Проверяет состояние компонента.
        /// </summary>
        public ObjectLiveState CheckAlive(TComponentInvokeType component, int index)
        {
            if (component == null)
            {
                return ObjectLiveState.Destroyed;
            }
            
            // If not alive.
            if (component.LiveState != ObjectLiveState.Alive)
            {
                Components[index] = null;
                return component.LiveState;
            }

            return ObjectLiveState.Alive;
        }

        private void ClearNulls()
        {
            Components.RemoveAll(x => x == null);
        }
    }
}