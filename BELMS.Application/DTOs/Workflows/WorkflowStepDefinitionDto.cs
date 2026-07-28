namespace BELMS.Application.DTOs.Workflows;

public class WorkflowStepDefinitionDto
{
    public Guid Id { get; set; }

    public int StepOrder { get; set; }

    public string Name { get; set; } = string.Empty;

    public string AssignedRole { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty;
}
