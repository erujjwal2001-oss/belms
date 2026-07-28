using BELMS.Application.DTOs.Dashboard;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IRoleDashboardService
{
    /// <summary>
    /// Builds the rich dashboard for the currently signed-in user based on their role.
    /// </summary>
    Task<Result<RoleDashboardDto>> GetOverviewAsync();
}
