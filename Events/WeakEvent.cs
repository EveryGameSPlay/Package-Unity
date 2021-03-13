using System;
using System.Collections.Generic;
using Egsp.Extensions.Collections;

namespace Egsp.Core
{
    public sealed class WeakEvent<TValue> where TValue : class
    {
        public delegate void Del(TValue value);

        private LinkedList<WeakReference<Del>> _subscribers;

        private WeakReference<TValue> _cachedValue;

        /// <summary>
        /// Нужно ли вызывать событие у подписчиков, которые подписались, но могли сделать это позднее.
        /// </summary>
        public bool RaiseLateSubscribers { get; set; } = true;

        public WeakEvent()
        {
            _subscribers = new LinkedList<WeakReference<Del>>();
        }

        public void Subscribe(Del del)
        {
            if (del == null)
                return;

            _subscribers.AddLast(new WeakReference<Del>(del));

            if (_cachedValue != null && RaiseLateSubscribers)
            {
                TValue target;
                if(_cachedValue.TryGetTarget(out target))
                    del.Invoke(target);
            }
        }

        public void Unsubscribe(Del del)
        {
            if (del == null)
                return;

            _subscribers.Remove(x =>
            {
                Del target;
                if (x.TryGetTarget(out target) && target == del)
                    return true;

                return false;
            });
        }

        public void Raise(TValue value)
        {
            foreach (var weakReference in _subscribers)
            {
                Del target;
                if(weakReference.TryGetTarget(out target) == false)
                    continue;

                target(value);
            }
            
            ClearNull();

            _cachedValue = new WeakReference<TValue>(value);
        }

        private void ClearNull()
        {
            _subscribers.RemoveAll(x =>
            {
                Del target;
                if (x.TryGetTarget(out target) == false)
                    return true;

                return false;
            });
        }

        public void ClearCachedValue()
        {
            _cachedValue = null;
        }
    }
}