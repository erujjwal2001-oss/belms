using BELMS.Application.DTOs.Excel;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IExcelService
{
    Task<byte[]> ExportEmployeesAsync();
    Task<byte[]> ExportAssetsAsync();
    Task<byte[]> ExportWorkflowsAsync();
    Task<byte[]> ExportAuditLogsAsync();
    Task<Result<EmployeeImportResultDto>> ImportEmployeesAsync(Stream fileStream);
}
