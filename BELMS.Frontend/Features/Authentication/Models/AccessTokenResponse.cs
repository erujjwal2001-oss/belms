namespace BELMS.Frontend.Features.Authentication.Models;

public class AccessTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}
