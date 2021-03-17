using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    public class SceneActivateAssistant : IDisposable
    {
        [NotNull] private readonly ILogger _logger;
        
        [NotNull] private readonly EventBus _bus;
        [NotNull] private readonly GameSceneManager _gameSceneManager;

        /// <summary>
        /// Кеш параметров активируемых сцен. Очищается при выгрузке сцены с соответствующим названием.
        /// </summary>
        [NotNull] private Dictionary<Scene, SceneParams> _sceneActivateParamsCache;

        /// <summary>
        /// Параметр нужный только при одиночном режиме загрузки сцены.
        /// </summary>
        [CanBeNull] private Tuple<string, SceneParams> _sceneSingleActivateParamsCache;

        public SceneActivateAssistant([NotNull] EventBus bus, [NotNull] GameSceneManager gameSceneManager,
            ILogger logger)
        {
            _bus = bus ?? throw new ArgumentNullException();
            _gameSceneManager = gameSceneManager ?? throw new ArgumentNullException();
            _logger = logger;

            _sceneActivateParamsCache = new Dictionary<Scene, SceneParams>();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            _bus.Raise<ISceneOperator>(o=>
                o.ActiveSceneChanged(newScene, GetActivateParams(newScene)));
            
            // Очищаем старые данные.
            RemoveActivateParams(newScene);
        }
        
        public void CacheActivateParams(Scene scene, SceneParams activateParams)
        {
            if (activateParams == null)
                return;
            
            if (_sceneActivateParamsCache.ContainsKey(scene))
            {
                _sceneActivateParamsCache[scene] = activateParams;
            }
            else
            {
                _sceneActivateParamsCache.Add(scene, activateParams);
            }
        }

        [NotNull]
        private SceneParams GetActivateParams(Scene scene, bool autoRemove = false)
        {
            // Если кеш не пуст, значит загружена одиночная сцена.
            if (_sceneSingleActivateParamsCache != null)
            {
                if (scene.name == _sceneSingleActivateParamsCache.Item1)
                {
                    var activateParams = _sceneSingleActivateParamsCache.Item2;
                    
                    if(activateParams == null)
                        activateParams = new EmptyParams();

                    _sceneSingleActivateParamsCache = null;

                    return activateParams;
                }
                else
                {
                    _logger.LogWarning("scene","Scene loaded, but single cache not cleared");
                }
            }
            
            if (_sceneActivateParamsCache.ContainsKey(scene))
            {
                var @params = _sceneActivateParamsCache[scene];
                if (autoRemove)
                    _sceneActivateParamsCache.Remove(scene);

                return @params;
            }

            return new EmptyParams();
        }

        private void RemoveActivateParams(Scene scene)
        {
            _sceneActivateParamsCache.Remove(scene);
        }

        public void CacheSingleActivateParams(string sceneName, [CanBeNull] SceneParams activateParams)
        {
            if(_sceneSingleActivateParamsCache != null)
                _logger.LogWarning("scene","Scene single params not removed before caching");
            
            _sceneSingleActivateParamsCache = new Tuple<string, SceneParams>(sceneName, activateParams);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RemoveActivateParams(scene);
        }

        public bool InjectActivateParams(Scene scene, [NotNull] SceneParams @params)
        {
            if (@params == null)
                return false;

            if (_sceneActivateParamsCache.ContainsKey(scene))
            {
                var activateParams = _sceneActivateParamsCache[scene];
                if (activateParams is EmptyParams)
                {
                    _sceneActivateParamsCache[scene] = @params;
                    return true;
                }
                
                return false;
            }

            _sceneActivateParamsCache.Add(scene, @params);

            return true;
        }
        
        public void Dispose()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}