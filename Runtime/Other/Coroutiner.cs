using System;
using System.Collections;
using Egsp.Core;
using JetBrains.Annotations;

namespace Egsp.Core
{
    /// <summary>
    /// Обычный синглтон, который позволяет запускать корутины для не MonoBehaviour.
    /// </summary>
    public sealed class Coroutiner : Singleton<Coroutiner>
    {
        /// <summary>
        /// Запускает корутину.
        /// </summary>
        private void StartRoutineInternal([NotNull]IEnumerator routine)
        {
            if(routine == null)
                throw new ArgumentNullException();
            
            StartCoroutine(routine);
        }

        /// <summary>
        /// Останавливает корутину.
        /// </summary>
        private void StopRoutineInternal([NotNull]IEnumerator routine)
        {
            if(routine == null)
                throw new ArgumentNullException();
            
            StopCoroutine(routine);
        }

        public static void StartRoutine(IEnumerator routine)
        {
            Instance.StartRoutineInternal(routine);
        }

        public static void StopRoutine(IEnumerator routine)
        {
            Instance.StopRoutineInternal(routine);
        }
    }
}