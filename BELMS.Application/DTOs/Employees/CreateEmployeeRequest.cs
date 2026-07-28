using BELMS.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Employees;

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = ValidationMessages.EmployeeCodeRequired)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.DepartmentRequired)]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.DesignationRequired)]
    public string Designation { get; set; } = string.Empty;

    public Guid? WorkflowDefinitionId { get; set; }
}
