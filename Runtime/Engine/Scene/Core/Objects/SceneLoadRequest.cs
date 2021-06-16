using System;
using System.Collections.Generic;
using Egsp.CSharp;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    /// <summary>
    /// Запрос на загрузку сцены, который ассоциирован с именем загружаемой сцены.
    /// Хранит в себе параметры.
    /// </summary>
    public sealed class SceneLoadRequest : DecoratorBase
    {
        public readonly string SceneName;
        
        /// <summary>
        /// Будет ли сцена активирована после загрузки.
        /// </summary>
        public readonly bool ActivateOnLoad;
            
        [NotNull]
        public SceneParams LoadParams { get; set; }
            
        /// <summary>
        /// Параметры, которые будут использованы при активации сцены после загрузки.
        /// </summary>
        [CanBeNull]
        public SceneParams ActivateParams { get; set; }
        
        public LoadSceneMode Mode { get; set; }
            
        /// <exception cref="Exception">Incorrect scene name.</exception>
        public SceneLoadRequest(string sceneName, bool activateOnLoad,
            [CanBeNull] SceneParams loadParams = null, [CanBeNull] SceneParams activateParams = null)
        {
            if(string.IsNullOrWhiteSpace(sceneName))
                throw new Exception($"Incorrect scene name:{sceneName}.");

            SceneName = sceneName;
            ActivateOnLoad = activateOnLoad;

            if (loadParams == null)
                loadParams = new EmptyParams();

            if (activateParams == null)
                activateParams = loadParams;

            LoadParams = loadParams;
            ActivateParams = activateParams;
        }
    }

    /// <summary>
    /// Прослойка функционала между запросом и пользователем. 
    /// </summary>
    public struct SceneLoadRequestProxy
    {
        private SceneLoadRequest _request;

        public SceneLoadRequestProxy(SceneLoadRequest request)
        {
            _request = request;
        }

        public void AddComponent<TComponent>(TComponent component)
            where TComponent : Component
        {
            _request.AddComponent(component);
        }
    }

    public class ActionsComponent : Component, IInvokableComponent
    {
        public Queue<Action> Actions = new Queue<Action>();
        
        public void Invoke()
        {
            foreach (var action in Actions)
            {
                action.Invoke();
            }
        }
    }
}