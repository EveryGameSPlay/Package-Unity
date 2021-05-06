using System;
using System.Collections;
using System.Collections.Generic;
using Egsp.Core;

namespace Egsp.CSharp
{
    public abstract class DecoratorBase : IEnumerable<Component>
    {
        private Dictionary<Type, Component> Components { get; set; } = new Dictionary<Type, Component>();
        
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
            Component.Attach(component, this);
            Components.Add(type, component);
        }

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

        private void RemoveComponentInternal<TComponent>(Type type, TComponent component)
            where TComponent : Component
        {
            Components.Remove(type);
            Component.Detach(component, this);
        }

        public Option<TComponent> GetComponent<TComponent>()
            where TComponent : Component, new()
        {
            if (Components.TryGetValue(typeof(TComponent), out var component))
            {
                return component as TComponent;
            }

            return Option<TComponent>.None;
        }
        
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
