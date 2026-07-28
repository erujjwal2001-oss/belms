using System.ComponentModel.DataAnnotations;

namespace BELMS.Frontend.Infrastructure.Api.Contracts;

public sealed class EmployeeDto
{
    public Guid Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public Guid? WorkflowInstanceId { get; set; }
}

public sealed class CreateEmployeeRequest
{
    [Required(ErrorMessage = "Employee code is required.")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required.")]
    public string Designation { get; set; } = string.Empty;

    public Guid? WorkflowDefinitionId { get; set; }
}

public sealed class UpdateEmployeeRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required.")]
    public string Designation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;
}
