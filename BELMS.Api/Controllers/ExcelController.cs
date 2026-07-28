using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/excel")]
[Authorize]
public class ExcelController(IExcelService excelService) : ApiControllerBase
{
    [HttpGet("employees/export")]
    [Authorize(Roles = "HR,Admin")]
    public async Task<IActionResult> ExportEmployees()
    {
        var bytes = await excelService.ExportEmployeesAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employees.xlsx");
    }

    [HttpPost("employees/import")]
    [Authorize(Roles = "HR,Admin")]
    public async Task<IActionResult> ImportEmployees(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        await using var stream = file.OpenReadStream();
        var result = await excelService.ImportEmployeesAsync(stream);
        return ProcessResult(result);
    }

    [HttpGet("assets/export")]
    [Authorize(Roles = "IT,Admin,HR")]
    public async Task<IActionResult> ExportAssets()
    {
        var bytes = await excelService.ExportAssetsAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "assets.xlsx");
    }

    [HttpGet("workflows/export")]
    [Authorize(Roles = "HR,Admin,Manager")]
    public async Task<IActionResult> ExportWorkflows()
    {
        var bytes = await excelService.ExportWorkflowsAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "workflows.xlsx");
    }

    [HttpGet("audit-logs/export")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportAuditLogs()
    {
        var bytes = await excelService.ExportAuditLogsAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "audit-logs.xlsx");
    }
}
