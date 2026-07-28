using BELMS.Frontend.Features.Dashboard.It.Models;

namespace BELMS.Frontend.Features.Dashboard.It.Services;

public interface IItDashboardService
{
    Task<ItDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
