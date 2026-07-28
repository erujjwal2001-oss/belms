using BELMS.Frontend.Features.Dashboard.Security.Models;

namespace BELMS.Frontend.Features.Dashboard.Security.Services;

public interface ISecurityDashboardService
{
    Task<SecurityDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
