using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Egsp.Core
{
    public class SceneInspectorParamsInjector : SerializedMonoBehaviour
    {
        [OdinSerialize] private SceneParams loadParams;
        [OdinSerialize] private SceneParams activateParams;
        
        private void Awake()
        {
            var tuple = GetSceneParams();
            GameSceneManager.Instance.InjectParamsToScene(gameObject.scene,
                loadParams: tuple.Item1,activateParams: tuple.Item2);
        }
        
        private Tuple<SceneParams, SceneParams> GetSceneParams()
        {
            return new Tuple<SceneParams, SceneParams>(loadParams, activateParams);
        }
    }
}