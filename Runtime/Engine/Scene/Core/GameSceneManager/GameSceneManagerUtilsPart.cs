using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    public enum SceneExistInBuildResult
    {
        Exist,
        NotExist,
        IncorrectName
    }
    
    public sealed partial class GameSceneManager
    {
        public static SceneExistInBuildResult SceneExistInBuild(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return SceneExistInBuildResult.IncorrectName;
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var lastSlash = scenePath.LastIndexOf("/", StringComparison.Ordinal);
                var sceneName =
                    scenePath.Substring(lastSlash + 1,
                        scenePath.LastIndexOf(".", StringComparison.Ordinal) - lastSlash - 1);

                if (String.Compare(name, sceneName, StringComparison.OrdinalIgnoreCase) == 0)
                    return SceneExistInBuildResult.Exist;
            }

            return SceneExistInBuildResult.NotExist;
        }

        /// <summary>
        /// True - если сцена существует. Дополнительно выводит сообщение в лог.
        /// </summary>
        public static bool SceneExistInBuild(string name, ILogger logger)
        {
            switch (SceneExistInBuild(name))
            {
                case SceneExistInBuildResult.Exist:
                    return true;
                case SceneExistInBuildResult.NotExist:
                    logger.Log($"Scene: {name} - doesnt exist in build.");
                    return false;
                case SceneExistInBuildResult.IncorrectName:
                    logger.Log("Incorrect scene name");
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }
    }
}