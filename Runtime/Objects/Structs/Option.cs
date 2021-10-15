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

        private readonly TValue _object;
        
        /// <summary>
        /// Имеется ли значение.
        /// </summary>
        public readonly bool IsSome;

        /// <summary>
        /// Отсутствует ли значение.
        /// </summary>
        public bool IsNone => !IsSome;

        public TValue Object => _object;

        public Option(TValue @object)
        {
            _object = @object;
            IsSome = @object != null;
        }

        public static implicit operator TValue(Option<TValue> option)
        {
            return option.Object;
        }
        
        public static implicit operator Option<TValue>(TValue value)
        {
            return new Option<TValue>(value);
        }

        public static implicit operator bool(Option<TValue> option)
        {
            return option.IsSome;
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
                return object.Equals(_object, other._object);
            else
                return IsSome == other.IsSome;
        }

        public override int GetHashCode()
        {
            return IsNone ? -1 : _object.GetHashCode();
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