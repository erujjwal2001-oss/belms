using BELMS.Frontend.Features.Authentication.Services;
using BELMS.Frontend.Features.Authentication.State;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using BELMS.Frontend.Infrastructure.Authentication.Logout;
using BELMS.Frontend.Infrastructure.Authentication.Session;
using BELMS.Frontend.Infrastructure.Authentication.Tokens;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BELMS.Frontend.Infrastructure.Authentication;

/// <summary>
/// Registers authentication services for direct API + token storage
/// </summary>
public static class AuthServiceRegistration
{
  public static IServiceCollection AddBelmsAuthentication(
      this IServiceCollection services,
      IConfiguration configuration,
      string apiBaseUrl)
  {
    // Use Redis only when a non-empty connection string is configured and reachable.
    var redisConnection = configuration.GetConnectionString("Redis");
    if (!string.IsNullOrWhiteSpace(redisConnection))
    {
      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = redisConnection;
      });
    }
    else
    {
      //  fallback when Redis is not configured or not running.
      services.AddDistributedMemoryCache();
    }

    services.AddHttpContextAccessor();

    var apiUri = new Uri(apiBaseUrl);

    // Typed HttpClients for auth operations against BELMS.Api (no bearer on login/refresh).
    services.AddHttpClient<IFAuthenticationService, AuthenticationService>(client =>
    {
      client.BaseAddress = apiUri;
    });

    services.AddHttpClient<TokenRefreshService>(client =>
    {
      client.BaseAddress = apiUri;
    });

    // Infrastructure: current user for services (ApiHandler, TokenService, RoleContext, etc.).
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddScoped<AuthenticatedUserHolder>();

    // Token storage backed by IDistributedCache (Redis when configured, otherwise in-memory).
    services.AddScoped<ITokenStore, RedisTokenStore>();
    services.AddScoped<TokenService>();
    services.AddScoped<LogoutService>();

    // Blazor UI auth state — same scoped instance for both concrete type and interface.
    services.AddScoped<CustomAuthStateProvider>();
    services.AddScoped<AuthenticationStateProvider>(sp =>
        sp.GetRequiredService<CustomAuthStateProvider>());

    return services;
  }

  /// <summary>
  /// Logs which token cache backend is active so connection issues are easier to diagnose.
  /// </summary>
  public static void LogTokenCacheBackend(this WebApplication app)
  {
    var redisConnection = app.Configuration.GetConnectionString("Redis");
    var logger = app.Services.GetRequiredService<ILoggerFactory>()
        .CreateLogger("BELMS.Authentication");

    if (string.IsNullOrWhiteSpace(redisConnection))
    {
      logger.LogWarning(
          "Redis connection string is empty — using in-memory token cache. " +
          "Set ConnectionStrings:Redis when Redis is available.");
    }
    else
    {
      logger.LogInformation("Token cache backend: Redis ({Endpoint})", redisConnection);
    }
  }
}
