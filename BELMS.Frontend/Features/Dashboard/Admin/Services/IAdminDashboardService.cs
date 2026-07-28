using BELMS.Frontend.Features.Dashboard.Admin.Models;

namespace BELMS.Frontend.Features.Dashboard.Admin.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
