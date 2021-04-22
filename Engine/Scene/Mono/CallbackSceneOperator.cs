using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    [Serializable]
    public class SceneParamsEvent : UnityEvent<SceneParams>
    {
            
    }
    
    /// <summary>
    /// Данный компонент будет подхвачен менеджером сцен при старте сцены.
    /// </summary>
    public class CallbackSceneOperator : MonoBehaviour, ISceneOperator
    {
        [SerializeField] private UnityEvent onActiveSceneChanged;
        [SerializeField] private UnityEvent onAfterSceneLoaded;
        [SerializeField] private UnityEvent onBeforeSceneUnload;
        [SerializeField] private UnityEvent onParentSceneActive;
        [SerializeField] private UnityEvent onParentSceneLoaded;
        [SerializeField] private UnityEvent onParentSceneUnload;
        
        
        [SerializeField] private SceneParamsEvent onActiveSceneChangedParams;
        [SerializeField] private SceneParamsEvent onAfterSceneLoadedParams;
        [SerializeField] private SceneParamsEvent onBeforeSceneUnloadParams;
        [SerializeField] private SceneParamsEvent onParentSceneActiveParams;
        [SerializeField] private SceneParamsEvent onParentSceneLoadedParams;
        [SerializeField] private SceneParamsEvent onParentSceneUnloadParams;

        private void Awake()
        {
            GameSceneManager.Instance.RegisterSceneOperator(this);
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