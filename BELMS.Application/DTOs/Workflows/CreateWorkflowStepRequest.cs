using BELMS.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Workflows;

public class CreateWorkflowStepRequest
{
    [Range(1, int.MaxValue)]
    public int StepOrder { get; set; }

    [Required(ErrorMessage = ValidationMessages.StepNameRequired)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string AssignedRole { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.StepTypeRequired)]
    public string StepType { get; set; } = string.Empty;
}
