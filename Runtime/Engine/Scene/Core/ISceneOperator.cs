
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    /// <summary>
    /// Оператор сцены в основном используется GameSceneManager для запуска стартовых событий сцены.
    /// Существовать может сколько угодно операторов, но смысла в этом много не будет, т.к. события везде одни.
    /// </summary>
    public interface ISceneOperator
    {
        Scene ParentScene { get; }

        void AfterSceneLoaded(Scene loadedScene, SceneParams @params);
        
        void ActiveSceneChanged(Scene newActiveScene, SceneParams @params);

        void BeforeSceneUnload(Scene scene, SceneParams @params);
    }

    public static class SceneOperatorExtensions
    {
        /// <summary>
        /// Является ли переданная сцена родительской для оператора.
        /// </summary>
        public static bool IsParentScene(this ISceneOperator sceneOperator,Scene scene)
        {
            return sceneOperator.ParentScene == scene;
        }
    }
}