using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.Hr.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Hr.Services;

public sealed class ApiHrDashboardService(IDashboardApiClient client) : IHrDashboardService
{
    public async Task<HrDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new HrDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load the HR dashboard right now."
            };
        }

        return new HrDashboardModel
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

public sealed class HrDashboardServiceDecorator(
    ApiHrDashboardService inner,
    IMemoryCache cache,
    ILogger<HrDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<HrDashboardModel>(cache, logger, currentUser, BelmsRoles.Hr),
      IHrDashboardService
{
    public Task<HrDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
