using BELMS.Frontend.Features.Dashboard.Employee.Models;

namespace BELMS.Frontend.Features.Dashboard.Employee.Services;

public interface IEmployeeDashboardService
{
    Task<EmployeeDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
