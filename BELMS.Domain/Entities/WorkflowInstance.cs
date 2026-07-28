using BELMS.Domain.Common;
using BELMS.Domain.Enums;

namespace BELMS.Domain.Entities;

public class WorkflowInstance : BaseEntity
{
    public WorkflowEntityType EntityType { get; set; }

    public Guid EntityId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public string WorkflowName { get; set; } = string.Empty;

    public int CurrentStep { get; set; } = 1;

    public WorkflowInstanceStatus Status { get; set; } = WorkflowInstanceStatus.Pending;

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public Employee? Employee { get; set; }

    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    public ICollection<WorkflowTask> Tasks { get; set; } = new List<WorkflowTask>();
}
