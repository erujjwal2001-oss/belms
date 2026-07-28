using BELMS.Frontend.Features.Dashboard.Admin.Models;
using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Admin.Services;

public sealed class ApiAdminDashboardService(IDashboardApiClient client) : IAdminDashboardService
{
    public async Task<AdminDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new AdminDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load the admin dashboard right now."
            };
        }

        return new AdminDashboardModel
        {
            WelcomeMessage = dto.WelcomeMessage,
            Stats = DashboardModelMapper.Stats(dto),
            PendingTasks = DashboardModelMapper.Tasks(dto),
            RecentActivity = DashboardModelMapper.Activity(dto),
            PrimaryChart = DashboardModelMapper.Chart(dto.PrimaryChart),
            RegistrationsChart = DashboardModelMapper.Chart(dto.SecondaryChart),
            QuickActions = DashboardModelMapper.QuickActions(dto)
        };
    }
}

public sealed class AdminDashboardServiceDecorator(
    ApiAdminDashboardService inner,
    IMemoryCache cache,
    ILogger<AdminDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<AdminDashboardModel>(cache, logger, currentUser, BelmsRoles.Admin),
      IAdminDashboardService
{
    public Task<AdminDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
