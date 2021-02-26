using System;

namespace Egsp.Core
{
    public readonly struct Op
    {
        public readonly OperationResultType Type;
        public readonly string ErrorMessage;
        
        public bool Ok => Type == OperationResultType.Correct;
        public bool NotOk => Type != OperationResultType.Correct;

        public Op(OperationResultType type,string errorMessage = null)
        {
            Type = type;
            
            if(errorMessage == null || type == OperationResultType.Correct)
                ErrorMessage = String.Empty;
            else
                ErrorMessage = errorMessage;
        }
        
        public Op(string errorMessage)
        {
            Type = OperationResultType.Error;
            ErrorMessage = errorMessage;
        }
    }
    
    public readonly struct Op<TResult>
    {
        public readonly OperationResultType Type;

        public readonly TResult Result;

        public readonly string ErrorMessage;

        public bool Ok => Type == OperationResultType.Correct;
        public bool NotOk => Type != OperationResultType.Correct;

        public Op(TResult result)
        {
            Type = OperationResultType.Correct;
            Result = result;
            
            ErrorMessage = String.Empty;
        }

        public Op(string errorMessage)
        {
            Type = OperationResultType.Error;
            ErrorMessage = errorMessage;

            Result = default(TResult);
        }
        
        public static Op<TResult> Correct(TResult result)
        {
            return new Op<TResult>(result);
        }

        public static Op<TResult> Error(string errorMessage)
        {
            return new Op<TResult>(errorMessage);
        }
    }
    
    public enum OperationResultType
    {
        Correct,
        Error
    }
}