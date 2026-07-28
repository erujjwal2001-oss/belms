using BELMS.Api.Contracts;
using BELMS.Domain.Common;
using BELMS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BELMS.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ProcessResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<object>.Success(null, "Operation successful"));
        }

        return MapFailure(result.Error);
    }

    protected IActionResult ProcessResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<T>.Success(result.Data!));
        }

        return MapFailure(result.Error);
    }

    private IActionResult MapFailure(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        var traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var response = ApiResponse<object>.Fail(
            error.Description,
            error.Code,
            error.Metadata
        );

        // Add extra structured info into Errors if needed
        response.Errors = error is ValidationError validationError
            ? validationError.Errors
            : error.Metadata;

        HttpContext.Response.Headers["x-trace-id"] = traceId;

        return StatusCode(statusCode, response);
    }
}