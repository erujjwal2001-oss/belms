namespace BELMS.Frontend.Infrastructure.Api;

/// <summary>Outcome of an API call carrying data or a human-readable error.</summary>
public sealed record ApiResult<T>(bool Success, T? Data, string? Error)
{
    public static ApiResult<T> Ok(T? data) => new(true, data, null);

    public static ApiResult<T> Fail(string error) => new(false, default, error);
}

/// <summary>Outcome of an API call with no payload.</summary>
public sealed record ApiResult(bool Success, string? Error)
{
    public static ApiResult Ok() => new(true, null);

    public static ApiResult Fail(string error) => new(false, error);
}
