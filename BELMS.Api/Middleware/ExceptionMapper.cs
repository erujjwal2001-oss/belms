using BELMS.Domain.Common.Constants;
using BELMS.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage;

namespace BELMS.Api.Middleware;

public static class ExceptionMapper
{
    public static ExceptionResult Map(Exception exception)
    {
        return exception switch
        {
            ValidationException ex => new ExceptionResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorCode = "VALIDATION_FAILED",
                Message = ValidationMessages.ValidationFailed,
                Errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray())
            },

            DomainException ex => new ExceptionResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            },

            UnauthorizedAccessException => new ExceptionResult
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                ErrorCode = "AUTH_UNAUTHORIZED",
                Message = "Unauthorized access"
            },

            _ => new ExceptionResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An unexpected error occurred"
            }
        };
    }
}