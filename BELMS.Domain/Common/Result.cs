using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, Error error, T? data, IReadOnlyList<string>? validationErrors)
            : base(isSuccess, error)
        {
            Data = data;
        }

        public static Result<T> Success(T data) => new(true, Error.None, data, null);

        public static new Result<T> Failure(Error error) => new(false, error, default, null);

        public static Result<T> ValidationFailure(List<string> errors)
       => new(false, Error.Validation("Validation.Failed", "Validation failed"), default, errors);

    }
}
