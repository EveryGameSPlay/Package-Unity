using System;

namespace Egsp.Core
{
    /// <summary>
    /// Объект-результат выполнения операции.
    /// Нужен для более мягкой обработки исключений.
    /// Может быть в двух состояниях: корректном и с ошибкой.
    /// Применение: получение состояния выполнения процедуры.
    /// </summary>
    public readonly struct Operation
    {
        public readonly OperationResultType Type;
        public readonly string ErrorMessage;
        
        public bool Ok => Type == OperationResultType.Correct;
        public bool NotOk => Type != OperationResultType.Correct;

        public Operation(OperationResultType type,string errorMessage = null)
        {
            Type = type;
            
            if(errorMessage == null || type == OperationResultType.Correct)
                ErrorMessage = String.Empty;
            else
                ErrorMessage = errorMessage;
        }
        
        public Operation(string errorMessage)
        {
            Type = OperationResultType.Error;
            ErrorMessage = errorMessage;
        }
    }
    
    /// <summary>
    /// Объект-результат выполнения операции.
    /// Нужен для более мягкой обработки исключений.
    /// Может быть в двух состояниях: корректном и с ошибкой.
    /// Применение: получение состояния выполнения процедуры.
    /// </summary>
    public readonly struct Operation<TResult>
    {
        public readonly OperationResultType Type;

        public readonly TResult Result;

        public readonly string ErrorMessage;

        public bool Ok => Type == OperationResultType.Correct;
        public bool NotOk => Type != OperationResultType.Correct;

        public Operation(TResult result)
        {
            Type = OperationResultType.Correct;
            Result = result;
            
            ErrorMessage = String.Empty;
        }

        public Operation(string errorMessage)
        {
            Type = OperationResultType.Error;
            ErrorMessage = errorMessage;

            Result = default(TResult);
        }
        
        public static Operation<TResult> Correct(TResult result)
        {
            return new Operation<TResult>(result);
        }

        public static Operation<TResult> Error(string errorMessage)
        {
            return new Operation<TResult>(errorMessage);
        }
    }
    
    /// <summary>
    /// Объект-результат выполнения операции.
    /// Нужен для более мягкой обработки исключений.
    /// Может быть в двух состояниях: корректном и с ошибкой.
    /// Применение: получение состояния выполнения процедуры.
    /// </summary>
    public readonly struct Operation<TResult, TError> 
    {
        public readonly OperationResultType Type;

        public readonly TResult Result;

        public readonly TError ErrorValue;

        public bool Ok => Type == OperationResultType.Correct;
        public bool NotOk => Type != OperationResultType.Correct;

        public Operation(TResult result)
        {
            Type = OperationResultType.Correct;
            Result = result;

            ErrorValue = default(TError);
        }

        public Operation(TError errorValue)
        {
            Type = OperationResultType.Error;
            ErrorValue = errorValue;

            Result = default(TResult);
        }
        
        public static Operation<TResult, TError> Correct(TResult result)
        {
            return new Operation<TResult, TError>(result);
        }

        public static Operation<TResult, TError> Error(TError errorValue)
        {
            return new Operation<TResult, TError>(errorValue);
        }

        public static explicit operator Operation(Operation<TResult, TError> operation)
        {
            if (operation.Ok)
                return new Operation(OperationResultType.Correct);
            else
                return new Operation(operation.ErrorValue.ToString());
        }

        public static explicit operator Operation<TResult>(Operation<TResult, TError> operation)
        {
            if (operation.Ok)
                return new Operation<TResult>(operation.Result);
            else
                return Operation<TResult>.Error(operation.ErrorValue.ToString());
        }
    }
    
    public enum OperationResultType
    {
        Correct,
        Error
    }

    public abstract class OperationError{ }

    public sealed class MessageError : OperationError
    {
        private readonly string _message;

        public MessageError(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message;
        }
    }
    
}