namespace BELMS.Application.DTOs.Excel;

public class EmployeeImportResultDto
{
    public int TotalRows { get; set; }

    public int ImportedCount { get; set; }

    public int SkippedCount { get; set; }

    public List<string> Errors { get; set; } = [];
}
