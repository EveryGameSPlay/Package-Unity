using System;
using System.Collections.Generic;
using Egsp.Extensions.Collections;

namespace Egsp.Core
{
    /// <summary>
    /// Структура хранит слабую ссылку на родителя делегата (Target).
    /// Сам же делегат просто присваивается полю.
    /// </summary>
    public struct WeakDelegateHandle<TValue>
    {
        // Есть предположение, что при ссылке делегатом на несколько методов,
        // данное событие будет вести себя некорректно.
        // Это в случае, если один из целевых объектов (getinvocationlist targets) отвалится.

        /// <summary>
        /// Целевой объект делегата. Данная структура нужна для объединения логики instance и static Target.
        /// </summary>
        private struct DelegateTarget
        {
            private static DelegateTarget _staticTarget = new DelegateTarget(true);
            /// <summary>
            /// Ссылка на экземпляр статичной цели.
            /// </summary>
            private static ref DelegateTarget StaticTarget => ref _staticTarget;
            
            /// <summary>
            /// Объект, на который ссылается делегат.
            /// </summary>
            private WeakReference<object> _target;
            
            private bool _isStaticTarget;

            /// <summary>
            /// Можно передать и делегат со статичной целью, конструктор сам проверит на отсутствие Target.
            /// </summary>
            public DelegateTarget(Action<TValue> del)
            {
                var delTarget = del.Target;
                if (del.Target == null)
                {
                    _isStaticTarget = true;
                    _target = null;
                }
                else
                {
                    _target = new WeakReference<object>(del.Target);
                    _isStaticTarget = false;
                }
            }

            public DelegateTarget(bool staticTarget)
            {
                _isStaticTarget = true;
                _target = null;
            }

            /// <summary>
            /// Статичная цель всегда будет считаться полной.
            /// </summary>
            public bool IsEmpty()
            {
                if (_isStaticTarget)
                    return false;

                object target;
                return !(_target?.TryGetTarget(out target) ?? true);
            }
        }
        
        /// <summary>
        /// Объект, на который ссылается делегат.
        /// </summary>
        private DelegateTarget _delTarget;
        
        /// <summary>
        /// Переданный делегат.
        /// </summary>
        private Action<TValue> _del;

        public bool Empty => _delTarget.IsEmpty();

        public WeakDelegateHandle(Action<TValue> del)
        {
            _delTarget = new DelegateTarget(del);
            _del = del;
        }

        public void Raise(TValue value)
        {
            object target;
            if (!_delTarget.IsEmpty())
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