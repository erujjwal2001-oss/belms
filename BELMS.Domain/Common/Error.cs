using BELMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Domain.Common
{
    public record Error(string Code, string Description, ErrorType Type, IDictionary<string, object>? Metadata = null)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", ErrorType.Failure);

        public static Error Failure
            (string code, string description, IDictionary<string, object>? metadata = null)
            => new(code, description, ErrorType.Failure, metadata);
        public static Error NotFound(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.NotFound, metadata);
        public static Error Conflict(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.Conflict, metadata);
        public static Error Validation(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.Validation, metadata);
        public static Error Unauthorized(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.Unauthorized, metadata);
        public static Error Forbidden(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.Forbidden, metadata);
        public static Error Unexpected(string code, string description, IDictionary<string, object>? metadata = null) => new(code, description, ErrorType.Unexpected, metadata);
    }

    public record ValidationError : Error
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationError(string code, string description, IDictionary<string, string[]> errors, IDictionary<string, object>? metadata = null)
            : base(code, description, ErrorType.Validation, metadata)
        {
            Errors = errors;
        }

        public static ValidationError Create(IDictionary<string, string[]> errors, IDictionary<string, object>? metadata = null)
            => new("ValidationError", "One or more validation errors occurred.", errors, metadata);
    }
}
