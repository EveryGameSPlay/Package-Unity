using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;

namespace Egsp.CSharp
{
    /// <summary>
    /// Базовый класс для объектов, к которым можно прикреплять компоненты.
    /// </summary>
    public abstract partial class DecoratorBase : IEnumerable<Component>
    {
        private Dictionary<Type, Component> Components { get; set; } = new Dictionary<Type, Component>();
        
        private List<ComponentGroup> Groups = new List<ComponentGroup>();

        private ObjectLiveState LiveState { get; set; } = ObjectLiveState.Alive;
        
        // ADD
        public AddComponentResult AddComponent<TComponent>(TComponent component)
            where TComponent : Component
        {
            var type = typeof(TComponent);
            if (Components.ContainsKey(type))
                return AddComponentResult.NotAdded;
            
            AddComponentInternal(type,component);
            return AddComponentResult.Added;
        }

        private void AddComponentInternal<TComponent>(Type type,TComponent component)
            where TComponent : Component
        {
            component.SetParent(this);
            Components.Add(type, component);
        }

        // REMOVE
        public RemoveComponentResult Remove<TComponent>()
            where TComponent : Component, new()
        {
            var type = typeof(TComponent);
            if (Components.TryGetValue(type, out var component))
            {
                RemoveComponentInternal(type, component);
                return RemoveComponentResult.Removed;
            }

            return RemoveComponentResult.NotFound;
        }
        
        /// <summary>
        /// При удалении компонента, этот компонент будет уничтожен.
        /// </summary>
        public RemoveComponentResult Remove(Component component)
        {
            var type = component.GetType();

            if (Components.TryGetValue(type, out var coincidence))
            {
                RemoveComponentInternal(type, coincidence);
                return RemoveComponentResult.Removed;
            }
    
            return RemoveComponentResult.NotFound;
        }

        private void RemoveComponentInternal<TComponent>(Type type, TComponent component)
            where TComponent : Component
        {
            Components.Remove(type);
            component.Destroy();
        }

        // GET
        public Option<TComponent> GetComponent<TComponent>()
            where TComponent : Component, new()
        {
            if (Components.TryGetValue(typeof(TComponent), out var component))
            {
                return component as TComponent;
            }

            return Option<TComponent>.None;
        }

        /// <summary>
        /// Уничтожает данный объект и все его компоненты.
        /// </summary>
        public void Destroy()
        {
            if (LiveState != ObjectLiveState.Alive)
                return;

            LiveState = ObjectLiveState.Destroying;
            OnDestroyInternal();  
            DestroyAllComponents();
            LiveState = ObjectLiveState.Destroyed;
        }

        private void DestroyAllComponents()
        {
            // Заносим все компоненты в список перед уничтожением, чтобы не получить исключение при итерации словаря.
            var components = Components.ToList();

            foreach (var component in components)
            {
                component.Value.Destroy();
            }
        }

        #region Enumerator
        public IEnumerator<Component> GetEnumerator()
        {
            foreach (var keyValuePair in Components)
            {
                yield return keyValuePair.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public enum AddComponentResult
        {
            Added,
            NotAdded
        }

        public enum RemoveComponentResult
        {
            Removed,
            NotFound
        }
    }
}
