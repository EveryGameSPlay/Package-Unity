using System;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    public sealed partial class GameSceneManager
    {
        public enum SceneExistInBuildResult
        {
            Exist,
            NotExist,
            IncorrectName
        }
        
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
    }
}