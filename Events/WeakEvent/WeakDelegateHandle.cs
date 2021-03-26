﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        // В данном случае хранится слабая ссылка только на экземпляр, метод которого нужно вызвать.
        // Поэтому, при очистке, на целевой объект не будет ничто ссылаться.
        // Переданный делегат не используется и не сохраняется, т.к. сам делегат содержит сильную ссылку на экземпляр.
        // Из делегата мы берем только ссылку на экземпляр и вызываемый метод.

        /// <summary>
        /// Объект, на который ссылается делегат.
        /// </summary>
        private DelegateTarget<TValue> _delTarget;

        /// <summary>
        /// Метод, который должен вызывать делегат.
        /// </summary>
        private MethodInfo _methodInfo;

        public bool Empty => _delTarget.IsEmpty();

        public WeakDelegateHandle(Action<TValue> del)
        {
            if (IsClosure(del))
                throw new InvalidOperationException();
            
            _delTarget = new DelegateTarget<TValue>(del);
            _methodInfo = del.Method;
        }

        public void Raise(TValue value)
        {
            if (!Empty)
            {
                _methodInfo.Invoke(_delTarget.Target, new object[] {value});
            }
        }

        public bool DelEqual(Action<TValue> del)
        {
            if (del.Target == _delTarget.Target && del.Method.Equals(_methodInfo))
                return true;
            return false;
        }

        public static bool IsClosure(Action<TValue> del)
        {
            return del.Target != null && Attribute.IsDefined(
                del.Method.DeclaringType, typeof(CompilerGeneratedAttribute));
        }
    }

    /// <summary>
    /// Целевой объект делегата. Данная структура нужна для объединения логики instance и static Target.
    /// </summary>
    public struct DelegateTarget<TValue>
    {
        private static DelegateTarget<TValue> _staticTarget = new DelegateTarget<TValue>(true);

        /// <summary>
        /// Ссылка на экземпляр статичной цели.
        /// </summary>
        private static ref DelegateTarget<TValue> StaticTarget => ref _staticTarget;

        /// <summary>
        /// Объект, на который ссылается делегат.
        /// </summary>
        private WeakReference<object> _target;

        private bool _isStaticTarget;

        private Option<object> TargetValue
        {
            get
            {
                object target;
                _target.TryGetTarget(out target);

                if (target == null)
                    return Option<object>.None;
                else
                    return target;
            }
        }

        // Если цель будет не статичной и не будет существовать, то произойдет ошибка доступа к значению.
        public object Target => _isStaticTarget ? null : TargetValue.Value;

        /// <summary>
        /// Можно передать и делегат со статичной целью, конструктор сам проверит на отсутствие Target.
        /// </summary>
        public DelegateTarget(Action<TValue> del)
        {
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

        public DelegateTarget(object customTarget)
        {
            _target = new WeakReference<object>(customTarget);
            _isStaticTarget = false;
        }

        /// <summary>
        /// Статичная цель всегда будет считаться полной.
        /// </summary>
        public bool IsEmpty()
        {
            if (_isStaticTarget)
                return false;

            return !(_target?.TryGetTarget(out _) ?? true);
        }
    }
}