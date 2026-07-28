using BELMS.Frontend.Features.Dashboard.Hr.Models;

namespace BELMS.Frontend.Features.Dashboard.Hr.Services;

public interface IHrDashboardService
{
    Task<HrDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
