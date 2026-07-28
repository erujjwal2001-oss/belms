using BELMS.Frontend.Features.Dashboard.Manager.Models;

namespace BELMS.Frontend.Features.Dashboard.Manager.Services;

public interface IManagerDashboardService
{
    Task<ManagerDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
