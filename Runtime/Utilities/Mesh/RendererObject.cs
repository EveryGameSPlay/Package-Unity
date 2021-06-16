using UnityEngine;

namespace Egsp.Core
{
    public class RendererObject : MonoBehaviour
    {
        /// <summary>
        /// Рендер компонент игрового объекта.
        /// </summary>
        public new Renderer renderer { get; set; }

        /// <summary>
        /// Возвращает посредника для установки значений материалу.
        /// </summary>
        public PropertyBlockProxy GetProxy() => new PropertyBlockProxy(renderer);
    }
}