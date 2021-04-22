using System;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    /// <summary>
    /// Исключение сообщающее о том, что сцена была загружена в нескольких экземплярах.
    /// </summary>
    public class SceneDuplicateException: Exception
    {
        public readonly Scene Scene;

        public SceneDuplicateException(Scene scene) : base($"Scene \"{scene.name}\" has already loaded!")
        {
            Scene = scene;
        }
    }
}