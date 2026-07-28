using System.Security.Claims;
using BELMS.Frontend.Infrastructure.Authentication.Session;

namespace BELMS.Frontend.Infrastructure.Authentication.CurrentUser;

/// <summary>
/// Reads the current user from HttpContext.User (cookie auth) with a scoped fallback
/// for the same Blazor circuit immediately after interactive login.
/// </summary>
public sealed class CurrentUserService(
    IHttpContextAccessor accessor,
    AuthenticatedUserHolder userHolder) : ICurrentUserService
{
  private readonly IHttpContextAccessor _accessor = accessor;
  private readonly AuthenticatedUserHolder _userHolder = userHolder;

  /// <summary>
  /// Resolves the active principal: cookie auth on HttpContext first, then scoped holder.
  /// </summary>
  private ClaimsPrincipal? User
  {
    get
    {
      var httpUser = _accessor.HttpContext?.User;
      if (httpUser?.Identity?.IsAuthenticated == true)
      {
        return httpUser;
      }

      return _userHolder.User;
    }
  }

  /// <inheritdoc />
  public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

  /// <inheritdoc />
  public string? UserId =>
      User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
      ?? User?.FindFirst("UserId")?.Value;

  /// <inheritdoc />
  public string? Username =>
      User?.FindFirst(ClaimTypes.Name)?.Value
      ?? User?.FindFirst("FullName")?.Value;

  /// <inheritdoc />
  public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

  /// <inheritdoc />
  public string? Role => Roles.FirstOrDefault();

  /// <inheritdoc />
  public IReadOnlyList<string> Roles =>
      User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
      ?? [];

  /// <inheritdoc />
  public ClaimsPrincipal? Principal => User;
}
