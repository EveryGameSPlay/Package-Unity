using UnityEngine;

namespace Egsp.Core.Ui
{
    /// <summary>
    /// Базовый класс для всех визуальных элементов интерфейса, содержащих логику.
    /// </summary>
    public abstract class Visual: MonoBehaviour, IVisual
    {
        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}