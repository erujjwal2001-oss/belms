using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using BELMS.Frontend.Models;

namespace BELMS.Frontend.Infrastructure.Authentication.Tokens;

/// <summary>
/// Reads and writes JWT pairs in Redis for the currently signed-in user.
/// </summary>
public sealed class TokenService(
    ICurrentUserService currentUser,
    ITokenStore store,
    TokenRefreshService refreshService)
{
  /// <summary>
  /// Stores a token bundle for the given user id (used after login).
  /// </summary>
  public Task SaveAsync(string userId, AuthResponse token) =>
      store.SaveAsync(userId, token);

  /// <summary>
  /// Removes cached tokens for the given user id (used on logout).
  /// </summary>
  public Task RemoveAsync(string userId) =>
      store.RemoveAsync(userId);

  /// <summary>
  /// Returns a valid access token for the current user, refreshing it when close to expiry.
  /// </summary>
  public async Task<string?> GetAccessTokenAsync()
  {
    var userId = currentUser.UserId;

    if (string.IsNullOrWhiteSpace(userId))
    {
      return null;
    }

    var token = await store.GetAsync(userId);

    if (token is null)
    {
      return null;
    }

    // Return the cached access token when it is still valid.
    if (token.AccessTokenExpiry > DateTime.UtcNow.AddMinutes(1))
    {
      return token.AccessToken;
    }

    // Attempt a silent refresh through BELMS.Api.
    var refreshed = await refreshService.RefreshAsync();

    if (!refreshed)
    {
      return null;
    }

    token = await store.GetAsync(userId);

    return token?.AccessToken;
  }
}
