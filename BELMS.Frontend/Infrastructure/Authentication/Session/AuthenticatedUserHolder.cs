using System.Security.Claims;

namespace BELMS.Frontend.Infrastructure.Authentication.Session;

/// <summary>
/// Scoped store for the authenticated user within the current Blazor circuit / HTTP request.
/// Used when cookie auth is not yet visible on HttpContext but login just succeeded.
/// </summary>
public sealed class AuthenticatedUserHolder
{
  private ClaimsPrincipal? _user;

  /// <summary>
  /// Returns the in-memory user for this scope, or null when nobody is signed in.
  /// </summary>
  public ClaimsPrincipal? User => _user;

  /// <summary>
  /// Saves the signed-in principal for the remainder of this scope.
  /// </summary>
  public void SetUser(ClaimsPrincipal user)
  {
    // Keep a copy so later reads are not affected if the source principal mutates.
    _user = user;
  }

  /// <summary>
  /// Clears any in-memory user (for example after logout).
  /// </summary>
  public void Clear()
  {
    _user = null;
  }
}
