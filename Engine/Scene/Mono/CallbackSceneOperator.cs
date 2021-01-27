using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    [Serializable]
    public class SceneParamsEvent : UnityEvent<SceneParams>
    {
            
    }
    // Для лучшего упраления лучше настроить очередь вызова скриптов. Оператор должен быть первее.
    public class CallbackSceneOperator : SerializedMonoBehaviour, ISceneOperator
    {
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onActiveSceneChanged;
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onAfterSceneLoaded;
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onBeforeSceneUnload;
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onParentSceneActive;
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onParentSceneLoaded;
        [FoldoutGroup("Without parameters")] [SerializeField]
        private UnityEvent onParentSceneUnload;
        
       
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onActiveSceneChangedParams;
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onAfterSceneLoadedParams;
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onBeforeSceneUnloadParams;
        
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onParentSceneActiveParams;
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onParentSceneLoadedParams;
        [FoldoutGroup("Parameterized")]
        [SerializeField] private SceneParamsEvent onParentSceneUnloadParams;

        private void Awake()
        {
            GameSceneManager.Instance.Bus.Subscribe<ISceneOperator>(this);
        }

        public Scene ParentScene => gameObject.scene;

        public void AfterSceneLoaded(Scene loadedScene, SceneParams @params)
        {
            onAfterSceneLoadedParams.Invoke(@params);
            onAfterSceneLoaded.Invoke();
            
            if(this.IsParentScene(loadedScene))
            {
                onParentSceneLoadedParams.Invoke(@params);
                onParentSceneLoaded.Invoke();
            }
        }

        public void ActiveSceneChanged(Scene newActiveScene, SceneParams @params)
        {
            onActiveSceneChangedParams.Invoke(@params);
            onActiveSceneChanged.Invoke();
            
            if(this.IsParentScene(newActiveScene))
            {
                onParentSceneActiveParams.Invoke(@params);
                onParentSceneActive.Invoke();
            }
        }

        public void BeforeSceneUnload(Scene scene, SceneParams @params)
        {
            onBeforeSceneUnloadParams.Invoke(@params);
            onBeforeSceneUnload.Invoke();
            
            if(this.IsParentScene(scene))
            {
                onParentSceneUnloadParams.Invoke(@params);
                onParentSceneUnload.Invoke();
            }
        }
    }
}