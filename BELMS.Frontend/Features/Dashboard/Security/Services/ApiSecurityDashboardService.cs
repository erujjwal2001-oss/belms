using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.Security.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Security.Services;

public sealed class ApiSecurityDashboardService(IDashboardApiClient client) : ISecurityDashboardService
{
    public async Task<SecurityDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new SecurityDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load the security dashboard right now."
            };
        }

        return new SecurityDashboardModel
        {
            WelcomeMessage = dto.WelcomeMessage,
            Stats = DashboardModelMapper.Stats(dto),
            PendingTasks = DashboardModelMapper.Tasks(dto),
            RecentActivity = DashboardModelMapper.Activity(dto),
            PrimaryChart = DashboardModelMapper.Chart(dto.PrimaryChart),
            QuickActions = DashboardModelMapper.QuickActions(dto),
            AccessReviews = DashboardModelMapper.AccessReviews(dto)
        };
    }
}

public sealed class SecurityDashboardServiceDecorator(
    ApiSecurityDashboardService inner,
    IMemoryCache cache,
    ILogger<SecurityDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<SecurityDashboardModel>(cache, logger, currentUser, BelmsRoles.Security),
      ISecurityDashboardService
{
    public Task<SecurityDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
