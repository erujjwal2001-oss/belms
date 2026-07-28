using BELMS.Frontend.Models;

namespace BELMS.Frontend.Features.Authentication.Services;

/// <summary>
/// Application-facing authentication operations (login, refresh, logout).
/// </summary>
public interface IFAuthenticationService
{
  /// <summary>Signs the user in via BELMS.Api and stores tokens in Redis.</summary>
  Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

  /// <summary>Rotates the access token using the stored refresh token.</summary>
  Task<bool> RefreshAsync(CancellationToken cancellationToken = default);

  /// <summary>Revokes tokens and clears the local session.</summary>
  Task<bool> LogoutAsync(CancellationToken cancellationToken = default);
}
