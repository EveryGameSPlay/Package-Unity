using System;

namespace Egsp.Core
{
    public readonly struct Option<TValue>
    {
        private static Option<TValue> _none = new Option<TValue>();
        public static ref readonly Option<TValue> None => ref _none;

        /// <summary>
        /// Имеется ли значение.
        /// </summary>
        public readonly bool IsSome;
        
        private readonly TValue _value;
        
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
            IsSome = true;
        }
        
        /// <summary>
        /// Создает новый экземпляр со значением. Альтернатива new OptionT(value).
        /// </summary>
        public static Option<TValue> Some(TValue value) => new Option<TValue>(value);

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
        
        public class OptionNoneValueAccessException : Exception
        {
        }
    }
}