using System;
using System.Collections.Generic;
using System.Reflection;
using Egsp.Extensions.Collections;
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Структура хранит слабую ссылку на родителя делегата (Target).
    /// Сам же делегат просто присваивается полю.
    /// </summary>
    public struct WeakDelegateHandle<TValue>
    {
        /// <summary>
        /// Объект, на который ссылается делегат.
        /// </summary>
        public WeakReference<object> DelTarget;
        
        /// <summary>
        /// Переданный делегат.
        /// </summary>
        private Action<TValue> _del;

        public bool Empty
        {
            get
            {
                object target;
                return !DelTarget.TryGetTarget(out target);
            }
        }

        public WeakDelegateHandle(Action<TValue> del)
        {
            DelTarget = new WeakReference<object>(del.Target);
            _del = del;
        }

        public void Raise(TValue value)
        {
            object target;
            if (DelTarget.TryGetTarget(out target))
            {
                _del.Invoke(value);
            }
            else
            {
                _del = null;
            }
        }
    }
    
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

            _subscribers.Remove(x => x.Empty);
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