using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Egsp.Core.Ui.Handlers
{
    /// <summary>
    /// Компонент реализующий интерфейс IPointDownHandler и вызывающий событие при нажатии.
    /// </summary>
    public class PointDownHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private UnityEvent onPointDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointDown.Invoke();            
        }
    }
}