using System.Security.Claims;
using BELMS.Frontend.Infrastructure.Authentication.Session;
using Microsoft.AspNetCore.Components.Authorization;

namespace BELMS.Frontend.Features.Authentication.State;

/// <summary>
/// Blazor AuthenticationStateProvider that mirrors the same user source as CurrentUserService.
/// UI components subscribe to this; infrastructure services use ICurrentUserService instead.
/// </summary>
public sealed class CustomAuthStateProvider(
    IHttpContextAccessor httpContextAccessor,
    AuthenticatedUserHolder userHolder) : AuthenticationStateProvider
{
  private static readonly AuthenticationState Anonymous =
      new(new ClaimsPrincipal(new ClaimsIdentity()));

  /// <summary>
  /// Returns the current authentication state for AuthorizeView, [Authorize], etc.
  /// </summary>
  public override Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    // Prefer the authenticated cookie principal when the browser already has a session.
    var httpUser = httpContextAccessor.HttpContext?.User;
    if (httpUser?.Identity?.IsAuthenticated == true)
    {
      return Task.FromResult(new AuthenticationState(httpUser));
    }

    // Fall back to the scoped holder right after interactive login in the same circuit.
    var heldUser = userHolder.User;
    if (heldUser?.Identity?.IsAuthenticated == true)
    {
      return Task.FromResult(new AuthenticationState(heldUser));
    }

    return Task.FromResult(Anonymous);
  }

  /// <summary>
  /// Notifies all listening Blazor components that auth state changed (for example after login).
  /// </summary>
  public void NotifyAuthenticationStateChanged()
  {
    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  }

  /// <summary>
  /// Notifies components that the user is now anonymous (for example after logout).
  /// </summary>
  public void NotifyUserLoggedOut()
  {
    NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
  }
}
