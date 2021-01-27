using UnityEngine;
using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    /// <summary>
    /// Обертка для сцены, несущая в себе дополнительную информацию.
    /// </summary>
    public sealed class SceneWrap
    {
        public readonly Scene Scene;
        public readonly LoadSceneMode LoadSceneMode;
            
        /// <summary>
        /// Время с момента создания обертки.
        /// Оно относительно старту игры.
        /// </summary>
        public readonly float CreationTime;

        public SceneWrap(Scene scene, LoadSceneMode loadSceneMode)
        {
            Scene = scene;
            LoadSceneMode = loadSceneMode;
            CreationTime = Time.time;
        }

        public bool SceneInstanceEquals(Scene scene)
        {
            // Сцена сравнивает handle (уникальное значение)
            return Scene.Equals(scene);
        }

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
                
            var wrap = obj as SceneWrap;
            if (wrap == null)
                return false;

            return Equals(wrap);
        }

        private bool Equals(SceneWrap other)
        {
            return Scene.Equals(other.Scene);
        }

        public override int GetHashCode()
        {
            return Scene.GetHashCode();
        }
        #endregion
    }
}