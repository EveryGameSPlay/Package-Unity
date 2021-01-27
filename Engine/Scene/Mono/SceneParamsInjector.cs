using System;
using UnityEngine;

namespace Egsp.Core
{
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