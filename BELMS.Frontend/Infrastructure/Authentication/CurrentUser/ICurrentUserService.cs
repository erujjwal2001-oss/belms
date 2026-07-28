using System.Security.Claims;

namespace BELMS.Frontend.Infrastructure.Authentication.CurrentUser;

/// <summary>
/// Convenient wrapper around the signed-in user for application/infrastructure services.
/// Reads from the ASP.NET cookie principal first, then the scoped in-memory holder.
/// </summary>
public interface ICurrentUserService
{
  /// <summary>True when a user is authenticated in the current scope.</summary>
  bool IsAuthenticated { get; }

  /// <summary>User id from NameIdentifier / UserId claim.</summary>
  string? UserId { get; }

  /// <summary>Display username from Name claim.</summary>
  string? Username { get; }

  /// <summary>Primary email address claim.</summary>
  string? Email { get; }

  /// <summary>First role claim, if any.</summary>
  string? Role { get; }

  /// <summary>All role claims for the current user.</summary>
  IReadOnlyList<string> Roles { get; }

  /// <summary>Resolved principal used by services in this request/circuit.</summary>
  ClaimsPrincipal? Principal { get; }
}
