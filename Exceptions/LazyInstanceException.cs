using System;

namespace Egsp.Core
{
    /// <summary>
    /// Исключение сообщающее о запрете отложенной инициализации объекта.
    /// </summary>
    public class LazyInstanceException : Exception
    {
        public LazyInstanceException(Type type)
            : base($"Lazy instance not allowed for {type.FullName}")
        {
            
        }
    }
}