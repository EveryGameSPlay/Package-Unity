using System;
using System.Collections;
using Egsp.Other;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    // TODO: Добавить класс кеша параметров сцены с двумя параметрами load и active.
    // На самом деле это не необходимо, поэтому отложено до надобности.
    
    /// <summary>
    /// <para>Класс нужен для работы с загрузкой сцен и передачей параметров в них.
    /// Также в классе определены полезные функции для получения различных данных.</para>
    /// 
    /// <para>Подписываться на данный менеджер нужно в Awake.
    /// Это необходимо для своевременного подхвата событий SceneManager!</para>
    /// </summary>
    public sealed partial class GameSceneManager : SingletonRaw<GameSceneManager>
    {
        [NotNull] private ILogger _logger;
        
        // Классы для обработки событий загрузки и активации сцен.
        [NotNull] private SceneLoadAssistant _sceneLoadAssistant;
        [NotNull] private SceneActivateAssistant _sceneActivateAssistant;
        
        // Поток событий менеджера.
        [NotNull] private EventBus Bus { get; set; }

        public GameSceneManager() : base()
        {
            _logger = Debug.unityLogger;
            
            Bus = new EventBus();
            
            _sceneLoadAssistant = new SceneLoadAssistant(Bus, this, _logger)
                {PreventOutsideLoading = PreventLoadType.Error};
            _sceneActivateAssistant = new SceneActivateAssistant(Bus, this, _logger);
        }

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (mode == LoadSceneMode.Single)
                LoadSceneSingle(sceneName, true);
            else
                LoadSceneAdditive(sceneName, true);
        }

        /// <summary>
        /// Запускает загрузку сцены в additive режиме и с параметрами.
        /// </summary>
        public void LoadSceneAdditive(string sceneName, bool activateOnLoad,[CanBeNull] SceneParams loadParams = null,
            [CanBeNull] SceneParams activateParams = null)
        {
            if (SceneExistInBuild(sceneName, _logger) == false)
                return;
            
            var loadRequest = new SceneLoadRequest(sceneName, activateOnLoad, loadParams,activateParams);
            loadRequest.Mode = LoadSceneMode.Additive;
            
            // Уведомляем обработчик загрузки о новом запросе.
            _sceneLoadAssistant.AcceptRequest(loadRequest);
            
            // Запускаем загрузку.
            var sceneRoutine = LoadSceneRoutine(sceneName, LoadSceneMode.Additive);
            Coroutiner.StartRoutine(sceneRoutine);
        }

        // При загрузке в режиме одиночной сцены, событие активации сработает первее чем событие загрузки.
        public void LoadSceneSingle(string sceneName, bool activateOnLoad, [CanBeNull] SceneParams loadParams = null,
            [CanBeNull] SceneParams activateParams = null)
        {
            if (SceneExistInBuild(sceneName, _logger) == false)
                return;
            
            var loadRequest = new SceneLoadRequest(sceneName, activateOnLoad, loadParams,activateParams);
            loadRequest.Mode = LoadSceneMode.Single;
            
            // Уведомляем обработчик загрузки о новом запросе.
            _sceneLoadAssistant.AcceptRequest(loadRequest);
            _sceneActivateAssistant.CacheSingleActivateParams(sceneName, activateParams);
            
            // Запускаем загрузку.
            var sceneRoutine = LoadSceneRoutine(sceneName, LoadSceneMode.Single);
            Coroutiner.StartRoutine(sceneRoutine);
        }

        /// <summary>
        /// Корутина загрузки сцены.
        /// </summary>
        private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode mode)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!loadOperation.isDone)
            {
                // TODO: Возможно стоит добавить лог сообщений о прогрессе загрузки.
                yield return null;
            }
        }

        /// <summary>
        /// Выгрузка сцены по названию.
        /// Не рекомендуется если загружено несколько сцен с одинаковым названием.
        /// Вместо этого используйте перегрузку с аргументом типа Scene (gameObject.scene).
        /// </summary>
        public void UnloadScene(string sceneName, SceneParams @params = null)
        {
            UnloadScene(SceneManager.GetSceneByName(sceneName), @params);
        }

        public void UnloadScene(Scene scene, SceneParams @params = null)
        {
            // Оповещаем всех операторов перед выгрузкой сцены.
            Bus.Raise<ISceneOperator>(o =>
                o.BeforeSceneUnload(scene, @params));

            SceneManager.UnloadSceneAsync(scene);
        }
        
        /// <summary>
        /// Устанавливает активную сцену.
        /// </summary>
        public bool SetActiveScene(string sceneName, SceneParams activateParams = null)
        {
            return SetActiveScene(SceneManager.GetSceneByName(sceneName), activateParams);
        }

        public bool SetActiveScene(Scene scene, SceneParams activateParams = null)
        {
            if (scene.IsValid())
            {
                // Кешируем параметры.
                if (activateParams != null)
                    _sceneActivateAssistant.CacheActivateParams(scene, activateParams);
                
                SceneManager.SetActiveScene(scene);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Добавляет параметры к сцене перед событием загрузки, если вызвано в методе Awake или Start объекта,
        /// находящегося на загружаемой сцене.
        /// </summary>
        public void InjectParamsToScene(Scene scene, SceneParams loadParams, SceneParams activateParams)
        {
            _sceneLoadAssistant.InjectLoadParams(scene, loadParams);
            _sceneActivateAssistant.InjectActivateParams(scene, activateParams);
        }

        /// <summary>
        /// Задает уровень ограничения загрузки сцен с помощью других классов. Таких как SceneManager.
        /// </summary>
        public void PreventOutsideLoading(PreventLoadType type)
        {
            _sceneLoadAssistant.PreventOutsideLoading = type;
        }

        /// <summary>
        /// Добавляет оператора сцена к потоку событий. 
        /// </summary>
        public void RegisterSceneOperator(ISceneOperator sceneOperator)
        {
            Bus.Subscribe<ISceneOperator>(sceneOperator);
        }
        
        
        public override void Dispose()
        {
            base.Dispose();
            
            _sceneLoadAssistant.Dispose();
            _sceneActivateAssistant.Dispose();
        }
    }
}