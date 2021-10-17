using System;

namespace Egsp.Core
{
    /// <summary>
    /// <para>При отсутствующем значении вызывает исключение на этапе создания экземпляра.</para>
    /// <para>Пустой конструктор нельзя использовать!</para>
    /// </summary>
    [Obsolete]
    public struct NotNull<TValue> where TValue : class
    {
        /// <summary>
        /// Если данное значение null, значит кто-то не использовал пустой конструктор.
        /// </summary>
        public readonly TValue Value;

        public NotNull(TValue value)
        {
            Value = value ?? throw new NullReferenceException();
        }

        public static explicit operator TValue(NotNull<TValue> optional)
        {
            return optional.Value;
        }
        
        public static implicit operator NotNull<TValue>(TValue value)
        {
            return new NotNull<TValue>(value);
        }
    }
}