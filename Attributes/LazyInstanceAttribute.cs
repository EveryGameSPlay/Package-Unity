using System;

namespace Egsp.Core
{
    /// <summary>
    /// Этим атрибутом обозначается класс с отложенной инициализацией.
    /// Атрибут лишь информирует и не влечет за собой каких-либо действий. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LazyInstanceAttribute : Attribute
    {
        public LazyInstanceAttribute()
        {
            AllowLazyInstance = true;
        }
        
        public LazyInstanceAttribute(bool allowLazyInstance)
        {
            AllowLazyInstance = allowLazyInstance;
        }
        
        /// <summary>
        /// Разрешена ли отложенная инициализация.
        /// </summary>
        public bool AllowLazyInstance { get; private set; }
    }
}