using System.Net.Http.Json;
using BELMS.Frontend.Infrastructure.Api;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using BELMS.Frontend.Models;

namespace BELMS.Frontend.Infrastructure.Authentication.Tokens;

/// <summary>
/// Refreshes expired access tokens using the refresh token stored in Redis.
/// Uses HttpClient directly to avoid a circular dependency with ApiHandler.
/// </summary>
public sealed class TokenRefreshService(
    HttpClient httpClient,
    ICurrentUserService currentUser,
    ITokenStore store)
{
  private static readonly SemaphoreSlim Lock = new(1, 1);

  /// <summary>
  /// Ensures the current user has a valid access token, refreshing via BELMS.Api when needed.
  /// </summary>
  public async Task<bool> RefreshAsync()
  {
    // Only one refresh per scope at a time to avoid duplicate API calls under load.
    await Lock.WaitAsync();

    try
    {
      var userId = currentUser.UserId;

      if (string.IsNullOrWhiteSpace(userId))
      {
        return false;
      }

      var token = await store.GetAsync(userId);

      if (token is null)
      {
        return false;
      }

      // Token still valid — no refresh required.
      if (token.AccessTokenExpiry > DateTime.UtcNow.AddMinutes(1))
      {
        return true;
      }

      var request = new RefreshTokenRequest
      {
        RefreshToken = token.RefreshToken
      };

      // Call refresh endpoint directly — no bearer token and no ApiHandler (avoids circular DI).
      var response = await httpClient.PostAsJsonAsync(ApiEndpoints.Refresh, request);

      if (!response.IsSuccessStatusCode)
      {
        await store.RemoveAsync(userId);
        return false;
      }

      var payload = await response.Content.ReadFromJsonAsync<ApiResponse<TokenRefreshDto>>();

      if (payload is not { IsSuccess: true, Data: not null })
      {
        await store.RemoveAsync(userId);
        return false;
      }

      // Persist the rotated token pair back into Redis for subsequent API calls.
      await store.SaveAsync(userId, new AuthResponse
      {
        AccessToken = payload.Data.AccessToken,
        RefreshToken = payload.Data.RefreshToken,
        AccessTokenExpiry = payload.Data.ExpiresAt,
        RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
      });

      return true;
    }
    finally
    {
      Lock.Release();
    }
  }
}

/// <summary>
/// Token payload returned by BELMS.Api refresh endpoint.
/// </summary>
internal sealed class TokenRefreshDto
{
  public string AccessToken { get; set; } = string.Empty;

  public string RefreshToken { get; set; } = string.Empty;

  public DateTime ExpiresAt { get; set; }
}
