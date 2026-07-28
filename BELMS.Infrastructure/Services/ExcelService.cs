using BELMS.Application.DTOs.Excel;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using ClosedXML.Excel;

namespace BELMS.Infrastructure.Services;

public class ExcelService(
    IEmployeeRepository employeeRepository,
    IAssetRepository assetRepository,
    IWorkflowRepository workflowRepository,
    IAuditLogRepository auditLogRepository,
    IWorkflowDefinitionRepository workflowDefinitionRepository,
    IWorkflowInstanceService workflowInstanceService) : IExcelService
{
    public async Task<byte[]> ExportEmployeesAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Employees");

        // Header row
        sheet.Cell(1, 1).Value = "Employee Code";
        sheet.Cell(1, 2).Value = "Full Name";
        sheet.Cell(1, 3).Value = "Email";
        sheet.Cell(1, 4).Value = "Department";
        sheet.Cell(1, 5).Value = "Designation";
        sheet.Cell(1, 6).Value = "Status";

        // Data rows from employee query
        var employees = employeeRepository.Query().OrderBy(x => x.EmployeeCode).ToList();
        var row = 2;
        foreach (var employee in employees)
        {
            sheet.Cell(row, 1).Value = employee.EmployeeCode;
            sheet.Cell(row, 2).Value = employee.FullName;
            sheet.Cell(row, 3).Value = employee.Email;
            sheet.Cell(row, 4).Value = employee.Department;
            sheet.Cell(row, 5).Value = employee.Designation;
            sheet.Cell(row, 6).Value = employee.Status.ToString();
            row++;
        }

        sheet.Columns().AdjustToContents();
        return WorkbookToBytes(workbook);
    }

    public async Task<byte[]> ExportAssetsAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Assets");

        sheet.Cell(1, 1).Value = "Asset Name";
        sheet.Cell(1, 2).Value = "Serial Number";
        sheet.Cell(1, 3).Value = "Asset Type";
        sheet.Cell(1, 4).Value = "Is Available";

        var assets = assetRepository.Query().OrderBy(x => x.AssetName).ToList();
        var row = 2;
        foreach (var asset in assets)
        {
            sheet.Cell(row, 1).Value = asset.AssetName;
            sheet.Cell(row, 2).Value = asset.SerialNumber;
            sheet.Cell(row, 3).Value = asset.AssetType.ToString();
            sheet.Cell(row, 4).Value = asset.IsAvailable ? "Yes" : "No";
            row++;
        }

        sheet.Columns().AdjustToContents();
        return WorkbookToBytes(workbook);
    }

    public async Task<byte[]> ExportWorkflowsAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Workflows");

        sheet.Cell(1, 1).Value = "Instance Id";
        sheet.Cell(1, 2).Value = "Workflow Name";
        sheet.Cell(1, 3).Value = "Entity Type";
        sheet.Cell(1, 4).Value = "Entity Id";
        sheet.Cell(1, 5).Value = "Current Step";
        sheet.Cell(1, 6).Value = "Status";
        sheet.Cell(1, 7).Value = "Started At";
        sheet.Cell(1, 8).Value = "Completed At";

        var instances = await workflowRepository.GetAllInstancesWithTasksAsync();
        var row = 2;
        foreach (var instance in instances)
        {
            sheet.Cell(row, 1).Value = instance.Id.ToString();
            sheet.Cell(row, 2).Value = instance.WorkflowName;
            sheet.Cell(row, 3).Value = instance.EntityType.ToString();
            sheet.Cell(row, 4).Value = instance.EntityId.ToString();
            sheet.Cell(row, 5).Value = instance.CurrentStep;
            sheet.Cell(row, 6).Value = instance.Status.ToString();
            sheet.Cell(row, 7).Value = instance.StartedAt;
            sheet.Cell(row, 8).Value = instance.CompletedAt?.ToString() ?? string.Empty;
            row++;
        }

        sheet.Columns().AdjustToContents();
        return WorkbookToBytes(workbook);
    }

    public async Task<byte[]> ExportAuditLogsAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("AuditLogs");

        sheet.Cell(1, 1).Value = "Entity Name";
        sheet.Cell(1, 2).Value = "Entity Id";
        sheet.Cell(1, 3).Value = "Action";
        sheet.Cell(1, 4).Value = "Performed By";
        sheet.Cell(1, 5).Value = "Old Values";
        sheet.Cell(1, 6).Value = "New Values";
        sheet.Cell(1, 7).Value = "Created At";

        var logs = await auditLogRepository.GetAllAsync();
        var row = 2;
        foreach (var log in logs)
        {
            sheet.Cell(row, 1).Value = log.EntityName;
            sheet.Cell(row, 2).Value = log.EntityId.ToString();
            sheet.Cell(row, 3).Value = log.Action;
            sheet.Cell(row, 4).Value = log.PerformedByUser?.FullName ?? string.Empty;
            sheet.Cell(row, 5).Value = log.OldValues ?? string.Empty;
            sheet.Cell(row, 6).Value = log.NewValues ?? string.Empty;
            sheet.Cell(row, 7).Value = log.CreatedAt;
            row++;
        }

        sheet.Columns().AdjustToContents();
        return WorkbookToBytes(workbook);
    }

    public async Task<Result<EmployeeImportResultDto>> ImportEmployeesAsync(Stream fileStream)
    {
        var result = new EmployeeImportResultDto();
        XLWorkbook workbook;

        // Parse uploaded xlsx stream
        try
        {
            workbook = new XLWorkbook(fileStream);
        }
        catch
        {
            return Result<EmployeeImportResultDto>.Failure(
                Error.Validation("Excel.InvalidFormat", ExcelMessages.InvalidFormat));
        }

        using (workbook)
        {
            var sheet = workbook.Worksheets.FirstOrDefault();
            if (sheet is null)
            {
                return Result<EmployeeImportResultDto>.Failure(
                    Error.Validation("Excel.EmptyFile", ExcelMessages.EmptyFile));
            }

            // Every imported employee starts the default onboarding workflow
            var defaultDefinition = await workflowDefinitionRepository.GetDefaultActiveAsync();
            if (defaultDefinition is null)
            {
                return Result<EmployeeImportResultDto>.Failure(
                    Error.NotFound("WorkflowDefinition.NoActive", WorkflowDefinitionMessages.NoActiveDefinition));
            }

            // Row 1 = headers; data starts at row 2
            var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;
            for (var row = 2; row <= lastRow; row++)
            {
                result.TotalRows++;
                var importRow = new EmployeeImportRowDto
                {
                    EmployeeCode = sheet.Cell(row, 1).GetString().Trim(),
                    FullName = sheet.Cell(row, 2).GetString().Trim(),
                    Email = sheet.Cell(row, 3).GetString().Trim().ToLowerInvariant(),
                    Department = sheet.Cell(row, 4).GetString().Trim(),
                    Designation = sheet.Cell(row, 5).GetString().Trim()
                };

                // Required fields check
                if (string.IsNullOrWhiteSpace(importRow.EmployeeCode)
                    || string.IsNullOrWhiteSpace(importRow.Email))
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Row {row}: Employee code and email are required.");
                    continue;
                }

                // Same domain rule as FluentValidation on manual create
                if (!importRow.Email.EndsWith("@laxmisunrise.com", StringComparison.OrdinalIgnoreCase))
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Row {row}: Email must end with @laxmisunrise.com.");
                    continue;
                }

                // Skip duplicates — do not fail entire import
                if (await employeeRepository.GetByEmployeeCodeAsync(importRow.EmployeeCode) is not null
                    || await employeeRepository.GetByEmailAsync(importRow.Email) is not null)
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Row {row}: Employee code or email already exists.");
                    continue;
                }

                var employee = new Employee
                {
                    EmployeeCode = importRow.EmployeeCode,
                    FullName = importRow.FullName,
                    Email = importRow.Email,
                    Department = importRow.Department,
                    Designation = importRow.Designation,
                    Status = EmployeeStatus.Pending
                };

                await employeeRepository.AddAsync(employee);
                await employeeRepository.SaveChangesAsync();

                // Auto-start onboarding workflow per imported row
                var instanceResult = await workflowInstanceService.StartFromDefinitionAsync(
                    employee.Id,
                    defaultDefinition.Id);

                if (instanceResult.IsFailure)
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Row {row}: {instanceResult.Error.Description}");
                    continue;
                }

                result.ImportedCount++;
            }
        }

        return Result<EmployeeImportResultDto>.Success(result);
    }

    // Serialize workbook to byte array for file download response
    private static byte[] WorkbookToBytes(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
