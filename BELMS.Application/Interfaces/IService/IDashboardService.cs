using BELMS.Application.DTOs.Dashboard;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IDashboardService
{
    Task<Result<DashboardDto>> GetDashboardAsync();
}
