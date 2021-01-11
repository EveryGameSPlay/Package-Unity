using System;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    public class SceneDuplicateException: Exception
    {
        public readonly Scene Scene;

        public SceneDuplicateException(Scene scene) : base($"Scene \"{scene.name}\" has already loaded!")
        {
            Scene = scene;
        }
    }
}