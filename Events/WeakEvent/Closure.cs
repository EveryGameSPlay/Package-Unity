using System;

namespace Egsp.Core
{
    public struct Closure<TValue>
    {
        private DelegateTarget<TValue> _hook;

        private Action<TValue> _closureAction;

        public bool Empty => _hook.IsEmpty();

        public Closure(object hook, Action<TValue> del)
        {
            _hook = new DelegateTarget<TValue>(hook);
            _closureAction = del;
        }
        
        public void Raise(TValue value)
        {
            if (!Empty)
            {
                _closureAction(value);
            }
            else
            {
                _closureAction = null;
            }
        }
        
        public bool DelEqual(Action<TValue> del)
        {
            return _closureAction.Equals(del);
        }

        public bool DelAndHookEqual(object closureHook, Action<TValue> del)
        {
            if (Empty)
                return false;

            if (_hook.Target == closureHook && _closureAction.Equals(del))
                return true;
            return false;
        }
    }
}