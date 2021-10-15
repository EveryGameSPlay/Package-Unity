using System;
using System.Collections.Generic;

namespace Egsp.Core
{
    /// <summary>
    /// Данный тип события хранит слабые ссылки на своих подписчиков (делегаты и замыкания).
    /// Также он позволяет получать данные с прошлого вызова всем поздним подписчикам.
    /// </summary>
    public sealed class WeakEvent<TValue>
    {
        private LinkedList<WeakDelegateHandle<TValue>> _subscribers;
        private LinkedList<Closure<TValue>> _closureSubscribers;

        private Option<TValue> _cachedValue;

        /// <summary>
        /// Нужно ли вызывать событие у подписчиков, которые подписались, но могли сделать это позднее.
        /// </summary>
        public bool RaiseLateSubscribers { get; set; } = true;

        public WeakEvent()
        {
            _subscribers = new LinkedList<WeakDelegateHandle<TValue>>();
            _closureSubscribers = new LinkedList<Closure<TValue>>();
        }

        /// <param name="closureHook">Если делегат окажется замыканием, то его жизнь будет связана с closureHook.</param>
        public void Subscribe(Action<TValue> del, object closureHook = null)
        {
            if (del == null)
                return;

            if (IsClosure(del))
            {
                _closureSubscribers.AddLast(new Closure<TValue>(closureHook, del));
            }
            else
            {
                _subscribers.AddLast(new WeakDelegateHandle<TValue>(del));
            }

            if (_cachedValue.IsSome && RaiseLateSubscribers)
            {
                del.Invoke(_cachedValue.Object);
            }
        }

        /// <param name="closureHook">Если делегат окажется замыканием, то поиск будет по closureHook.</param>
        public void Unsubscribe(object closureHook, Action<TValue> del)
        {
            if (del == null)
                return;

            if (IsClosure(del))
            {
                _closureSubscribers.Remove(x => x.DelAndHookEqual(closureHook, del));
            }
            else
            {
                _subscribers.Remove(x => x.DelEqual(del));
            }
        }

        /// <summary>
        /// Вызывает событие и кеширует значение.
        /// </summary>
        public void Raise(TValue value)
        {
            RaiseOnce(value);
            _cachedValue = value;
        }

        /// <summary>
        /// Вызывает событие без последующего кеширования значения.
        /// </summary>
        /// <param name="value"></param>
        public void RaiseOnce(TValue value)
        {
            foreach (var weakDelegateHandle in _subscribers)
            {
                weakDelegateHandle.Raise(value);
            }

            foreach (var closureSubscriber in _closureSubscribers)
            {
                closureSubscriber.Raise(value);
            }
            
            ClearNullSubscribers();
        }

        private void ClearNullSubscribers()
        {
            _subscribers.RemoveAll(x => x.Empty);
            _closureSubscribers.RemoveAll(x => x.Empty);
        }

        public void ClearCachedValue()
        {
            _cachedValue = Option<TValue>.None;
        }

        private bool IsClosure(Action<TValue> del)
        {
            return WeakDelegateHandle<TValue>.IsClosure(del);
        }
    }
}