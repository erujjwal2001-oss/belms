using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Employees;

public class UpdateEmployeeRequest
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    [Required]
    public string Designation { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;
}
