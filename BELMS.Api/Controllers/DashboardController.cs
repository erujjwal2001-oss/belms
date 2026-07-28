using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController(
    IDashboardService dashboardService,
    IRoleDashboardService roleDashboardService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await dashboardService.GetDashboardAsync();
        return ProcessResult(result);
    }

    /// <summary>
    /// Rich, role-aware dashboard for the signed-in user, composed on the server.
    /// </summary>
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await roleDashboardService.GetOverviewAsync();
        return ProcessResult(result);
    }
}
