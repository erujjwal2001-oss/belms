using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Shared.Decorators;

/// <summary>
/// Base Decorator that adds short-lived per-user caching and logging around any role
/// dashboard service, without the wrapped service knowing about either concern.
/// </summary>
public abstract class CachingDashboardDecorator<TModel>(
    IMemoryCache cache,
    ILogger logger,
    ICurrentUserService currentUser,
    string role)
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(20);

    protected async Task<TModel> GetOrLoadAsync(
        Func<CancellationToken, Task<TModel>> loader,
        CancellationToken cancellationToken)
    {
        var key = $"dashboard:{role}:{currentUser.UserId ?? "anon"}";

        if (cache.TryGetValue(key, out TModel? cached) && cached is not null)
        {
            logger.LogDebug("Dashboard cache hit for {Role} ({User}).", role, currentUser.UserId);
            return cached;
        }

        logger.LogInformation("Loading {Role} dashboard for {User}.", role, currentUser.UserId);
        var model = await loader(cancellationToken);
        cache.Set(key, model, CacheDuration);
        return model;
    }
}
