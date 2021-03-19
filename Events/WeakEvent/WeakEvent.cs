using System;
using System.Collections.Generic;
using Egsp.Extensions.Collections;

namespace Egsp.Core
{
    public sealed class WeakEvent<TValue>
    {
        private LinkedList<WeakDelegateHandle<TValue>> _subscribers;

        private Option<TValue> _cachedValue;

        /// <summary>
        /// Нужно ли вызывать событие у подписчиков, которые подписались, но могли сделать это позднее.
        /// </summary>
        public bool RaiseLateSubscribers { get; set; } = true;

        public WeakEvent()
        {
            _subscribers = new LinkedList<WeakDelegateHandle<TValue>>();
        }

        public void Subscribe(Action<TValue> del)
        {
            // Переданный делегат будет жить в пределах вызывающего метода
            // и соответственно будет хранить ссылку на экземпляр.
            
            if (del == null)
                return;

            _subscribers.AddLast(new WeakDelegateHandle<TValue>(del));

            if (_cachedValue.IsSome && RaiseLateSubscribers)
            {
                del.Invoke(_cachedValue.Value);
            }
        }

        public void Unsubscribe(Action<TValue> del)
        {
            if (del == null)
                return;

            _subscribers.Remove(x => x.DelEqual(del));
        }

        public void Raise(TValue value)
        {
            foreach (var weakDelegateHandle in _subscribers)
            {
                weakDelegateHandle.Raise(value);
            }

            ClearNull();

            _cachedValue = value;
        }

        private void ClearNull()
        {
            _subscribers.RemoveAll(x => x.Empty);
        }

        public void ClearCachedValue()
        {
            _cachedValue = Option<TValue>.None;
        }
    }
}