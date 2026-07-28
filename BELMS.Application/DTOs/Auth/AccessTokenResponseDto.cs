namespace BELMS.Application.DTOs.Auth;

public class AccessTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}
