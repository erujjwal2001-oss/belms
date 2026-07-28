namespace BELMS.Application.DTOs.Workflows;

public class WorkflowInstanceDto
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
