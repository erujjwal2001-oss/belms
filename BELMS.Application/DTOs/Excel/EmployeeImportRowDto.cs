namespace BELMS.Application.DTOs.Excel;

public class EmployeeImportRowDto
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;
}
