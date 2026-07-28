using BELMS.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Workflows;

public class CreateWorkflowDefinitionRequest
{
    [Required(ErrorMessage = ValidationMessages.WorkflowNameRequired)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.WorkflowStepsRequired)]
    [MinLength(1, ErrorMessage = ValidationMessages.WorkflowStepsRequired)]
    public List<CreateWorkflowStepRequest> Steps { get; set; } = [];
}
