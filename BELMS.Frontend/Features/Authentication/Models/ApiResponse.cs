namespace BELMS.Frontend.Features.Authentication.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

    public string? ErrorCode { get; set; }

    public T? Data { get; set; }

    public object? Errors { get; set; }
}
