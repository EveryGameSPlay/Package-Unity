using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Класс отражает все статические функции класса ApplicationManager.
    /// Нужен для работы с UnityEvents.
    /// </summary>
    public class ApplicationManagerReflection : MonoBehaviour
    {
        public void Close() => ApplicationManager.Close();
    }
}