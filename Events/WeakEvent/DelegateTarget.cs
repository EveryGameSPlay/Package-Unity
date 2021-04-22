using System;

namespace Egsp.Core
{
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