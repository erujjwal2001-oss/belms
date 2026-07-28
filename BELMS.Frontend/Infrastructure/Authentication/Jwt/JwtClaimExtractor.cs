using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BELMS.Frontend.Infrastructure.Authentication.Jwt;

/// <summary>
/// Reads identity claims from a JWT access token returned by BELMS.Api.
/// </summary>
public static class JwtClaimExtractor
{
  /// <summary>
  /// Parses the JWT and returns its claims without validating the signature.
  /// Validation is performed by BELMS.Api; the frontend only needs identity data for cookies/UI.
  /// </summary>
  public static IReadOnlyList<Claim> ExtractClaims(string accessToken)
  {
    // Read the token structure and pull out the claim set issued by the API.
    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(accessToken);
    return jwt.Claims.ToList();
  }

  /// <summary>
  /// Builds a cookie-authenticated principal from JWT claims.
  /// </summary>
  public static ClaimsPrincipal CreatePrincipal(string accessToken, string authenticationScheme)
  {
    // Turn JWT claims into a principal ASP.NET cookie auth and Blazor can both understand.
    var claims = ExtractClaims(accessToken);
    var identity = new ClaimsIdentity(claims, authenticationScheme);
    return new ClaimsPrincipal(identity);
  }
}
