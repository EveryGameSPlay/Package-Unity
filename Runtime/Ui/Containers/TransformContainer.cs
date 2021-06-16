using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Egsp.Core.Utils;

namespace Egsp.Core
{
    /// <summary>
    /// Контейнер, которому для размещения нужен лишь компонент Transform.
    /// </summary>
    public class TransformContainer : MonoBehaviour, IContainer, IContextEntityDetector
    {
        public bool worldPositionStays;

        private LinkedList<object> _container = new LinkedList<object>();
        
        public IEventBus DetectBus { get; set; }

        public TObject PutPrefab<TObject>(TObject prefab) where TObject : Component
        {
            var inst = Instantiate(prefab);
            return Put(inst);
        }

        public TObject Put<TObject>(TObject instance) where TObject : Component
        {
            // Parent
            instance.transform.SetParent(transform, worldPositionStays);
            
            // Context
            CheckContextDependency(instance);
            
            _container.AddLast(instance);
            return instance;
        }

        private void CheckContextDependency(Component monoBehaviour)
        {
            var contextEntity = monoBehaviour as IContextEntity;

            if (contextEntity != null)
                DetectBus?.Raise<IContext>(x => x.AddEntity(contextEntity));
        }

        public void Clear()
        {
            _container.Clear();
            transform.DestroyAllChildrens();
        }

        public void DestroyLast()
        {
            var last = _container.Last.Value as Component;

            if (last != null)
            {
                _container.RemoveLast();
                Destroy(last.gameObject);
            }
        }

        public IEnumerable<TObject> GetEnumerable<TObject>()
        {
            return _container.Cast<TObject>();
        }
    }
}