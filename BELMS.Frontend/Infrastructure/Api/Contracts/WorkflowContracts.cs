using System.ComponentModel.DataAnnotations;

namespace BELMS.Frontend.Infrastructure.Api.Contracts;

public sealed class WorkflowDefinitionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public List<WorkflowStepDefinitionDto> Steps { get; set; } = [];
}

public sealed class WorkflowStepDefinitionDto
{
    public Guid Id { get; set; }

    public int StepOrder { get; set; }

    public string Name { get; set; } = string.Empty;

    public string AssignedRole { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty;
}

public sealed class CreateWorkflowDefinitionRequest
{
    [Required(ErrorMessage = "Workflow name is required.")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<CreateWorkflowStepRequest> Steps { get; set; } = [];
}

public sealed class CreateWorkflowStepRequest
{
    public int StepOrder { get; set; }

    [Required(ErrorMessage = "Step name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Assigned role is required.")]
    public string AssignedRole { get; set; } = "HR";

    [Required(ErrorMessage = "Step type is required.")]
    public string StepType { get; set; } = "Approval";
}

public sealed class UpdateWorkflowDefinitionRequest
{
    [Required(ErrorMessage = "Workflow name is required.")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public sealed class WorkflowInstanceDto
{
    public Guid Id { get; set; }

    public string EntityType { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public string WorkflowName { get; set; } = string.Empty;

    public int CurrentStep { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public List<WorkflowTaskDto> Tasks { get; set; } = [];
}

public sealed class WorkflowTaskDto
{
    public Guid Id { get; set; }

    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty;

    public string AssignedRole { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? Comments { get; set; }

    public Guid? CompletedByUserId { get; set; }

    public DateTime? CompletedAt { get; set; }
}

public sealed class CompleteTaskRequest
{
    public string? Comments { get; set; }
}

public static class WorkflowRoles
{
    public static readonly IReadOnlyList<string> All =
        ["HR", "Manager", "IT", "Security", "Admin"];
}
