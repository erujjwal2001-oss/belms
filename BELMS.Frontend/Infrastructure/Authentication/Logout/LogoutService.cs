using BELMS.Frontend.Features.Authentication.State;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using BELMS.Frontend.Infrastructure.Authentication.Session;
using BELMS.Frontend.Infrastructure.Authentication.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace BELMS.Frontend.Infrastructure.Authentication.Logout;

/// <summary>
/// Signs the user out locally, clears Redis tokens, and refreshes Blazor auth state.
/// </summary>
public sealed class LogoutService(
    ICurrentUserService currentUser,
    ITokenStore tokenStore,
    IHttpContextAccessor httpContextAccessor,
    AuthenticatedUserHolder userHolder,
    CustomAuthStateProvider authStateProvider,
    NavigationManager navigationManager)
{
  private const string CookieScheme = "Cookies";

  /// <summary>
  /// Removes stored tokens, clears cookie auth, notifies Blazor, and navigates to login.
  /// </summary>
  public async Task LogoutAsync()
  {
    var userId = currentUser.UserId;

    // Remove access/refresh tokens from Redis so ApiHandler cannot reuse them.
    if (!string.IsNullOrEmpty(userId))
    {
      await tokenStore.RemoveAsync(userId);
    }

    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext is not null)
    {
      // Delete the ASP.NET authentication cookie from the browser session.
      await httpContext.SignOutAsync(CookieScheme);
    }

    // Clear the in-memory principal used during the current Blazor circuit.
    userHolder.Clear();

    // Tell AuthorizeView / route guards that the user is no longer signed in.
    authStateProvider.NotifyUserLoggedOut();

    // Full reload ensures the next page load starts with a clean unauthenticated context.
    navigationManager.NavigateTo("/login", forceLoad: true);
  }
}
