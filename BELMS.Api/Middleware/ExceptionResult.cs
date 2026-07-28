namespace BELMS.Api.Middleware;

public class ExceptionResult
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; } = default!;
    public string Message { get; set; } = default!;
    public object? Errors { get; set; }
}