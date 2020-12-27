using UnityEngine;

namespace Egsp.Utils.GameObjectUtilities
{
    public static class GameObjectUtils
    {
        
        public static void DestroyAllChildrens(this Transform transform)
        {
            for (var i = transform.childCount-1; i >-1 ; i--)
            {
                SafeDestroy(transform.GetChild(i).gameObject);
            }
        }
        
        public static T SafeDestroy<T>(T obj) where T : UnityEngine.Object
        {
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(obj);
            else
                UnityEngine.Object.Destroy(obj);
     
            return null;
        }
        
        public static T SafeDestroyComponent<T>(T component) where T : Component
        {
            if (component != null)
                SafeDestroy(component.gameObject);
            return null;
        }
    }
}