using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.Manager.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Manager.Services;

public sealed class ApiManagerDashboardService(IDashboardApiClient client) : IManagerDashboardService
{
    public async Task<ManagerDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new ManagerDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load the manager dashboard right now."
            };
        }

        return new ManagerDashboardModel
        {
            WelcomeMessage = dto.WelcomeMessage,
            Stats = DashboardModelMapper.Stats(dto),
            PendingTasks = DashboardModelMapper.Tasks(dto),
            RecentActivity = DashboardModelMapper.Activity(dto),
            PrimaryChart = DashboardModelMapper.Chart(dto.PrimaryChart),
            QuickActions = DashboardModelMapper.QuickActions(dto)
        };
    }
}

public sealed class ManagerDashboardServiceDecorator(
    ApiManagerDashboardService inner,
    IMemoryCache cache,
    ILogger<ManagerDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<ManagerDashboardModel>(cache, logger, currentUser, BelmsRoles.Manager),
      IManagerDashboardService
{
    public Task<ManagerDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
