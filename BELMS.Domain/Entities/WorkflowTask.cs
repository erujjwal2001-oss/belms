using BELMS.Domain.Common;
using BELMS.Domain.Enums;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Domain.Entities;

public class WorkflowTask : BaseEntity
{
    public Guid WorkflowInstanceId { get; set; }

    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty;

    public Role AssignedRole { get; set; }

    public string? Comments { get; set; }

    public DomainTaskStatus Status { get; set; } = DomainTaskStatus.Pending;

    public Guid? CompletedByUserId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public WorkflowInstance WorkflowInstance { get; set; } = null!;

    public User? CompletedByUser { get; set; }

    public ICollection<WorkflowAttachment> Attachments { get; set; } = new List<WorkflowAttachment>();
}
