using BELMS.Frontend.Features.Dashboard.Core;
using BELMS.Frontend.Features.Dashboard.Employee.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Decorators;
using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.Caching.Memory;

namespace BELMS.Frontend.Features.Dashboard.Employee.Services;

/// <summary>API-backed employee dashboard built from the server overview payload.</summary>
public sealed class ApiEmployeeDashboardService(IDashboardApiClient client) : IEmployeeDashboardService
{
    public async Task<EmployeeDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.GetOverviewAsync();
        var dto = result.Data;
        if (!result.Success || dto is null)
        {
            return new EmployeeDashboardModel
            {
                WelcomeMessage = result.Error ?? "Unable to load your dashboard right now."
            };
        }

        return new EmployeeDashboardModel
        {
            WelcomeMessage = dto.WelcomeMessage,
            Stats = DashboardModelMapper.Stats(dto),
            PendingTasks = DashboardModelMapper.Tasks(dto),
            RecentActivity = DashboardModelMapper.Activity(dto),
            PrimaryChart = DashboardModelMapper.Chart(dto.PrimaryChart),
            QuickActions = DashboardModelMapper.QuickActions(dto),
            Notifications = DashboardModelMapper.Notifications(dto),
            WorkflowSteps = DashboardModelMapper.WorkflowSteps(dto)
        };
    }
}

/// <summary>Caching + logging Decorator over <see cref="ApiEmployeeDashboardService"/>.</summary>
public sealed class EmployeeDashboardServiceDecorator(
    ApiEmployeeDashboardService inner,
    IMemoryCache cache,
    ILogger<EmployeeDashboardServiceDecorator> logger,
    ICurrentUserService currentUser)
    : CachingDashboardDecorator<EmployeeDashboardModel>(cache, logger, currentUser, BelmsRoles.Employee),
      IEmployeeDashboardService
{
    public Task<EmployeeDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync(inner.GetDashboardAsync, cancellationToken);
}
