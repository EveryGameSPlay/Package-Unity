using System;
using System.Collections;
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
        [NotNull] private SceneLoadHandler _sceneLoadHandler;
        [NotNull] private SceneActivateHandler _sceneActivateHandler;
        
        // Поток событий менеджера.
        [NotNull] private EventBus Bus { get; set; }

        public GameSceneManager() : base()
        {
            _logger = Debug.unityLogger;
            
            Bus = new EventBus();
            
            _sceneLoadHandler = new SceneLoadHandler(Bus, this, _logger)
                {PreventOutsideLoading = PreventLoadType.Error};
            _sceneActivateHandler = new SceneActivateHandler(Bus, this, _logger);
        }

        public Option<SceneLoadRequestProxy> LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (mode == LoadSceneMode.Single)
                return LoadSceneSingle(sceneName, true);
            else
                return LoadSceneAdditive(sceneName, true);
        }

        /// <summary>
        /// Загржает сцену с компонентом набора команд.
        /// </summary>
        public void LoadSceneWith(string sceneName, ActionsComponent actions, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Option<SceneLoadRequestProxy> loadProxy; 
            if (mode == LoadSceneMode.Single)
                loadProxy = LoadSceneSingle(sceneName, true);
            else
                loadProxy = LoadSceneAdditive(sceneName, true);

            if (loadProxy.IsSome)
                loadProxy.Value.AddComponent(actions);
        }

        /// <summary>
        /// Запускает загрузку сцены в additive режиме и с параметрами.
        /// </summary>
        public Option<SceneLoadRequestProxy> LoadSceneAdditive(
            string sceneName, bool activateOnLoad,[CanBeNull] SceneParams loadParams = null,
            [CanBeNull] SceneParams activateParams = null)
        {
            if (SceneExistInBuild(sceneName, _logger) == false)
                return Option<SceneLoadRequestProxy>.None;
            
            var loadRequest = new SceneLoadRequest(sceneName, activateOnLoad, loadParams,activateParams);
            loadRequest.Mode = LoadSceneMode.Additive;
            
            // Уведомляем обработчик загрузки о новом запросе.
            _sceneLoadHandler.AcceptRequest(loadRequest);
            
            // Запускаем загрузку.
            var sceneRoutine = LoadSceneRoutine(sceneName, LoadSceneMode.Additive);
            Coroutiner.StartRoutine(sceneRoutine);

            return new SceneLoadRequestProxy(loadRequest);
        }

        // При загрузке в режиме одиночной сцены, событие активации сработает первее чем событие загрузки.
        public Option<SceneLoadRequestProxy> LoadSceneSingle(
            string sceneName, bool activateOnLoad, [CanBeNull] SceneParams loadParams = null,
            [CanBeNull] SceneParams activateParams = null)
        {
            if (SceneExistInBuild(sceneName, _logger) == false)
                return Option<SceneLoadRequestProxy>.None;
            
            var loadRequest = new SceneLoadRequest(sceneName, activateOnLoad, loadParams,activateParams);
            loadRequest.Mode = LoadSceneMode.Single;
            
            // Уведомляем обработчик загрузки о новом запросе.
            _sceneLoadHandler.AcceptRequest(loadRequest);
            _sceneActivateHandler.CacheSingleActivateParams(sceneName, activateParams);
            
            // Запускаем загрузку.
            var sceneRoutine = LoadSceneRoutine(sceneName, LoadSceneMode.Single);
            Coroutiner.StartRoutine(sceneRoutine);

            return new SceneLoadRequestProxy(loadRequest);
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
                    _sceneActivateHandler.CacheActivateParams(scene, activateParams);
                
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
            _sceneLoadHandler.InjectLoadParams(scene, loadParams);
            _sceneActivateHandler.InjectActivateParams(scene, activateParams);
        }

        /// <summary>
        /// Задает уровень ограничения загрузки сцен с помощью других классов. Таких как SceneManager.
        /// </summary>
        public void PreventOutsideLoading(PreventLoadType type)
        {
            _sceneLoadHandler.PreventOutsideLoading = type;
        }

        /// <summary>
        /// Добавляет оператора сцена к потоку событий. 
        /// </summary>
        public void RegisterSceneOperator(ISceneOperator sceneOperator)
        {
            Bus.Subscribe<ISceneOperator>(sceneOperator);
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();
            
            _sceneLoadHandler.Dispose();
            _sceneActivateHandler.Dispose();
        }
    }
}