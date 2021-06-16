using System;

namespace Egsp.Core
{
    /// <summary>
    /// <para>Данная структура является аналогом option из языка F#.
    /// Позволяет вернуть результат или ничего.</para>
    ///
    /// <para>Данный тип сигнализирует о том, что результат может вернуться в двух вариантах. 
    /// Варианты: Some and None. Да, мы можем вернуть просто null, но как мы тогда узнаем,
    /// что метод его будет возвращать. А вдруг не будет, или все же будет...</para>
        
    /// <para>Почему null отстой ->
    /// https://www.westerndevs.com/Fsharp/Functional-programming/maybe-null-is-not-an-option/ </para>
        
    /// <para>Tony Hoare calls null references his billion dollar mistake. Using null values (NULL, Null, nil, etc)
    /// makes code harder to maintain and to understand.</para>
    /// </summary>
    public readonly struct Option<TValue>
    {
        private static Option<TValue> _none = new Option<TValue>();
        public static ref readonly Option<TValue> None => ref _none;

        /// <summary>
        /// Имеется ли значение.
        /// </summary>
        public readonly bool IsSome;
        
        private readonly TValue _value;

        public bool IsNone => !IsSome;
        
        public TValue Value
        {
            get
            {
                if (IsSome)
                    return _value;
                else
                    throw new OptionNoneValueAccessException();
            }
        }

        public Option(TValue value)
        {
            _value = value;
            IsSome = value != null;
        }

        public static explicit operator TValue(Option<TValue> optional)
        {
            return optional.Value;
        }
        
        public static implicit operator Option<TValue>(TValue value)
        {
            return new Option<TValue>(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is Option<TValue>)
                return this.Equals((Option<TValue>)obj);
            else
                return false;
        }
        public bool Equals(Option<TValue> other)
        {
            if (IsSome && other.IsSome)
                return object.Equals(_value, other._value);
            else
                return IsSome == other.IsSome;
        }

        public override string ToString()
        {
            return IsSome ? "Some" : "None";
        }

        public class OptionNoneValueAccessException : Exception
        {
        }
    }
}