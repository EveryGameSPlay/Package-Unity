using System;

namespace Egsp.Core
{
    public readonly struct Operation<TResult>
    {
        public readonly OperationResultType Type;

        public readonly TResult Result;

        public readonly string ErrorMessage;

        public bool Ok => Type == OperationResultType.Correct;

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
    
    public enum OperationResultType
    {
        Correct,
        Error
    }
}