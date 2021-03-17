using System;
using System.Collections;
using Egsp.Core;
using JetBrains.Annotations;

namespace Egsp.Other
{
    public class Coroutiner : Singleton<Coroutiner>
    {
        /// <summary>
        /// Запускает корутину.
        /// </summary>
        public void StartRoutineInternal([NotNull]IEnumerator routine)
        {
            if(routine == null)
                throw new ArgumentNullException();
            
            StartCoroutine(routine);
        }

        /// <summary>
        /// Останавливает корутину.
        /// </summary>
        public void StopRoutineInternal([NotNull]IEnumerator routine)
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