using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;

namespace BELMS.Frontend.Features.Dashboard.Services;

/// <summary>
/// Supplies role and profile information to dashboard components from the signed-in user.
/// </summary>
public sealed class RoleContext(ICurrentUserService currentUser) : IRoleContext
{
  /// <summary>
  /// Returns the user's BELMS role, defaulting to Employee when missing or unknown.
  /// </summary>
  public Task<string> GetRoleAsync(CancellationToken cancellationToken = default)
  {
    var role = currentUser.Role;
    if (string.IsNullOrWhiteSpace(role) || !BelmsRoles.All.Contains(role))
    {
      role = BelmsRoles.Employee;
    }

    return Task.FromResult(role);
  }

  /// <summary>
  /// Returns a friendly display name for the signed-in user.
  /// </summary>
  public Task<string> GetDisplayNameAsync(CancellationToken cancellationToken = default)
  {
    var name = currentUser.Username
        ?? currentUser.Email
        ?? "User";

    return Task.FromResult(name);
  }

  /// <summary>
  /// Returns the signed-in user's email address.
  /// </summary>
  public Task<string> GetEmailAsync(CancellationToken cancellationToken = default)
  {
    var email = currentUser.Email ?? string.Empty;
    return Task.FromResult(email);
  }
}
