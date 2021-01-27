
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    public interface ISceneOperator
    {
        Scene ParentScene { get; }

        void AfterSceneLoaded(Scene loadedScene, SceneParams @params);
        
        void ActiveSceneChanged(Scene newActiveScene, SceneParams @params);

        void BeforeSceneUnload(Scene scene, SceneParams @params);
    }

    public static class SceneOperatorExtensions
    {
        public static bool IsParentScene(this ISceneOperator sceneOperator,Scene scene)
        {
            return sceneOperator.ParentScene == scene;
        }
    }
}