using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.It.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.It.Services;

public sealed class ApiItDashboardService(IDashboardApiClient client) : IItDashboardService
{
    public async Task<ItDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new ItDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load the IT dashboard right now."
            };
        }

        return new ItDashboardModel
        {
            WelcomeMessage = dto.WelcomeMessage,
            Stats = DashboardModelMapper.Stats(dto),
            PendingTasks = DashboardModelMapper.Tasks(dto),
            RecentActivity = DashboardModelMapper.Activity(dto),
            PrimaryChart = DashboardModelMapper.Chart(dto.PrimaryChart),
            QuickActions = DashboardModelMapper.QuickActions(dto),
            AssetSummary = DashboardModelMapper.AssetSummary(dto)
        };
    }
}

public sealed class ItDashboardServiceDecorator(
    ApiItDashboardService inner,
    IMemoryCache cache,
    ILogger<ItDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<ItDashboardModel>(cache, logger, currentUser, BelmsRoles.It),
      IItDashboardService
{
    public Task<ItDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
