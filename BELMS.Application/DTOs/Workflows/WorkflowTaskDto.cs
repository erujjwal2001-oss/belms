namespace BELMS.Application.DTOs.Workflows;

public class WorkflowTaskDto
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

    public List<WorkflowAttachmentDto> Attachments { get; set; } = [];
}
