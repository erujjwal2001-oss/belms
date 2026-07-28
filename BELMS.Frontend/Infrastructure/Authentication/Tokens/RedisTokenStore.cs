using BELMS.Frontend.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BELMS.Frontend.Infrastructure.Authentication.Tokens;

/// <summary>
/// Stores JWT pairs in Redis (via IDistributedCache) keyed by user id.
/// </summary>
public sealed class RedisTokenStore(IDistributedCache cache) : ITokenStore
{
  private readonly IDistributedCache _cache = cache;

  /// <summary>
  /// Builds the Redis key for a user's cached auth tokens.
  /// </summary>
  private static string BuildKey(string userId) => $"auth-token:{userId}";

  /// <summary>
  /// Reads the cached token bundle for the given user, or null when nothing is stored.
  /// </summary>
  public async Task<AuthResponse?> GetAsync(string userId)
  {
    var data = await _cache.GetStringAsync(BuildKey(userId));

    if (data is null)
    {
      return null;
    }

    return JsonSerializer.Deserialize<AuthResponse>(data);
  }

  /// <summary>
  /// Deletes the cached tokens for the given user (logout / failed refresh).
  /// </summary>
  public async Task RemoveAsync(string userId)
  {
    await _cache.RemoveAsync(BuildKey(userId));
  }

  /// <summary>
  /// Serializes and stores the token bundle for the given user in Redis.
  /// </summary>
  public async Task SaveAsync(string userId, AuthResponse token)
  {
    var data = JsonSerializer.Serialize(token);

    await _cache.SetStringAsync(
        BuildKey(userId),
        data,
        new DistributedCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        });
  }
}
