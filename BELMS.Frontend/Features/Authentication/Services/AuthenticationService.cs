using BELMS.Frontend.Features.Authentication.Services;
using BELMS.Frontend.Features.Authentication.State;
using BELMS.Frontend.Infrastructure.Api;
using BELMS.Frontend.Infrastructure.Authentication.Jwt;
using BELMS.Frontend.Infrastructure.Authentication.Session;
using BELMS.Frontend.Infrastructure.Authentication.Tokens;
using BELMS.Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BELMS.Frontend.Features.Authentication.Services;

/// <summary>
/// Handles login, refresh, and logout by calling BELMS.Api directly and storing tokens in Redis.
/// </summary>
public sealed class AuthenticationService(
    HttpClient httpClient,
    ITokenStore tokenStore,
    IHttpContextAccessor httpContextAccessor,
    AuthenticatedUserHolder userHolder,
    CustomAuthStateProvider authStateProvider,
    TokenRefreshService tokenRefreshService) : IFAuthenticationService
{
  private const string CookieScheme = "Cookies";

    /// <summary>
    /// Authenticates against BELMS.Api, stores tokens in Redis, and establishes the local session.
    /// </summary>
    public async Task<LoginResult> LoginAsync(
      LoginRequest request,
      CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            ApiEndpoints.Login,
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new LoginResult(false, null);
        }

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<TokenApiDto>>(
            cancellationToken: cancellationToken);

        if (payload is not { IsSuccess: true, Data: not null })
        {
            return new LoginResult(false, null);
        }

        var token = new AuthResponse
        {
            AccessToken = payload.Data.AccessToken,
            RefreshToken = payload.Data.RefreshToken,
            AccessTokenExpiry = payload.Data.ExpiresAt,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        var principal = JwtClaimExtractor.CreatePrincipal(
            token.AccessToken,
            CookieScheme);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? principal.FindFirst("UserId")?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return new LoginResult(false, null);
        }

        await tokenStore.SaveAsync(userId, token);

        return new LoginResult(true, principal);
    }

    /// <summary>
    /// Asks BELMS.Api to rotate tokens using the refresh token stored in Redis.
    /// </summary>
    public async Task<bool> RefreshAsync(CancellationToken cancellationToken = default)
  {
    return await tokenRefreshService.RefreshAsync();
  }

  /// <summary>
  /// Revokes the refresh token on the API, clears Redis, and signs the user out locally.
  /// </summary>
  public async Task<bool> LogoutAsync(CancellationToken cancellationToken = default)
  {
    var httpContext = httpContextAccessor.HttpContext;
    var userId = userHolder.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        ?? httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    string? refreshToken = null;
    if (!string.IsNullOrWhiteSpace(userId))
    {
      var stored = await tokenStore.GetAsync(userId);
      refreshToken = stored?.RefreshToken;
      await tokenStore.RemoveAsync(userId);
    }

    if (!string.IsNullOrWhiteSpace(refreshToken))
    {
      // Best-effort server-side revocation; local session is cleared regardless.
      _ = await httpClient.PostAsync(
          $"{ApiEndpoints.Logout}?refreshToken={Uri.EscapeDataString(refreshToken)}",
          content: null,
          cancellationToken);
    }

    userHolder.Clear();
    authStateProvider.NotifyUserLoggedOut();

    return true;
  }
}

/// <summary>
/// Token payload shape returned by BELMS.Api login/refresh endpoints.
/// </summary>
internal sealed class TokenApiDto
{
  public string AccessToken { get; set; } = string.Empty;

  public string RefreshToken { get; set; } = string.Empty;

  public DateTime ExpiresAt { get; set; }
}

public sealed record LoginResult(
    bool Success,
    ClaimsPrincipal? Principal);
