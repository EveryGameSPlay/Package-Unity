using System;
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Данный класс вносит свои параметры в менеджер сцен перед стартом сцены.
    /// Для определения вносимых параметров нужно наследоваться от данного класса и переопределить метод GetSceneParams.
    /// </summary>
    public abstract class SceneParamsInjector : MonoBehaviour
    {
        private void Awake()
        {
            var tuple = GetSceneParams();
            GameSceneManager.Instance.InjectParamsToScene(gameObject.scene,
                loadParams: tuple.Item1,activateParams: tuple.Item2);
            
        }

        protected abstract Tuple<SceneParams, SceneParams> GetSceneParams();
    }
}