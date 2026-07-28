namespace BELMS.Application.DTOs.Workflows;

public class WorkflowDefinitionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public List<WorkflowStepDefinitionDto> Steps { get; set; } = [];
}
