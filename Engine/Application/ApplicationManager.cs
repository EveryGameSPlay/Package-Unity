using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Здесь собраны высокоуровневые функции для контроля приложения.
    /// </summary>
    public static class ApplicationManager
    {
        /// <summary>
        /// Закрывает приложение.
        /// Останавливает редактор.
        /// </summary>
        public static void Close()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}