using System;

namespace Egsp.Core
{
    /// <summary>
    /// <para>Данный объект "обещание" позволяет получить данные, которые будут лишь в будущем.</para>
    /// <para>Если возникнет ошибка, то "обещание" вернет объект ошибки.</para>
    /// 
    /// <para>После вызова одного из событий, все подписчики будут отписаны.</para>
    /// </summary>
    public class Promise<R, F>
    {
        protected event Action<R> OnResult = delegate(R obj) {  };
        protected event Action<F> OnFail = delegate(F obj) {  };

        // Одно из свойств будет пусто. Option используется для обозначения пустоты, в частности структур.
        private Option<R> _result = Option<R>.None;
        private Option<F> _fail = Option<F>.None;

        public PromiseState State { get; protected set; } = PromiseState.Waiting;

        public R Result
        {
            set
            {
                if(State == PromiseState.Fail)
                    throw new InvalidOperationException();
                
                _result = value;
                State = PromiseState.Result;
                
                OnResult(_result.option);
                
                ClearAllSubscribers();
            }
        }

        public F Fail
        {
            set
            {
                if (State == PromiseState.Result)
                    throw new InvalidOperationException();
                
                _fail = value;
                State = PromiseState.Fail;
                
                OnFail(_fail.option);
                
                ClearAllSubscribers();
            }
        }
        
        public void GetResult(Action<R> resultAction)
        {
            OnResult += resultAction;

            if (_result.IsSome)
                InvokeResult(_result.option);
        }

        public void GetFail(Action<F> failAction)
        {
            OnFail += failAction;

            if (_fail.IsSome)
                InvokeFail(_fail.option);
        }
        
        protected void InvokeResult(in R result)
        {
            OnResult(result);
            ClearAllSubscribers();
        }

        protected void InvokeFail(in F fail)
        {
            OnFail(fail);
            ClearAllSubscribers();
        }

        protected void ClearAllSubscribers()
        {
            OnResult = delegate(R r) {  };
            OnFail = delegate(F f) {  };
        }

        public enum PromiseState
        {
            Waiting,
            Result,
            Fail
        }
    }
    
    /// <summary>
    /// <para>Данный объект "обещание" позволяет получить данные, которые будут лишь в будущем.</para>
    /// <para>Если возникнет ошибка, то "обещание" вернет объект ошибки.</para>
    /// 
    /// <para>После вызова одного из событий, все подписчики будут отписаны.</para>
    /// </summary>
    public class Promise<T> : Promise<T, string>
    {
        
    }
}