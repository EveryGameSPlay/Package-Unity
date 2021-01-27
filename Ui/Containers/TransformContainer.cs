using System.Collections.Generic;
using System.Linq;
using Egsp.Utils.GameObjectUtilities;
using UnityEngine;

namespace Egsp.Core
{
    public class TransformContainer : MonoBehaviour, IContainer, IContextEntityDetector
    {
        public bool worldPositionStays;

        private List<object> _container = new List<object>();
        
        public IEventBus DetectBus { get; set; }

        public TObject PutPrefab<TObject>(TObject prefab) where TObject : MonoBehaviour
        {
            var inst = Instantiate(prefab);
            return Put(inst);
        }

        public TObject Put<TObject>(TObject instance) where TObject : MonoBehaviour
        {
            // Parent
            instance.transform.SetParent(transform, worldPositionStays);
            
            // Context
            CheckContextDependency(instance);
            
            _container.Add(instance);
            return instance;
        }

        private void CheckContextDependency(MonoBehaviour monoBehaviour)
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

        public IEnumerable<TObject> GetEnumerable<TObject>()
        {
            return _container.Cast<TObject>();
        }
    }
}