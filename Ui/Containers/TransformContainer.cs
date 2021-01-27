using System.Collections.Generic;
using System.Linq;
using Egsp.Utils.GameObjectUtilities;
using UnityEngine;

namespace Egsp.Core
{
    public class TransformContainer : MonoBehaviour, IContainer
    {
        public bool worldPositionStays;

        private List<object> _container = new List<object>();

        public TObject PutPrefab<TObject>(TObject prefab) where TObject : MonoBehaviour
        {
            var inst = Instantiate(prefab);
            inst.transform.SetParent(transform,worldPositionStays);
            
            _container.Add(inst);
            return inst;
        }

        public TObject Put<TObject>(TObject instance) where TObject : MonoBehaviour
        {
            instance.transform.SetParent(transform, worldPositionStays);
            
            _container.Add(instance);
            return instance;
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