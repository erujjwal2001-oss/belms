namespace BELMS.Application.DTOs.Workflows;

public class WorkflowAttachmentDto
{
    public Guid Id { get; set; }

    public Guid WorkflowTaskId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public Guid UploadedByUserId { get; set; }

    public DateTime UploadedAt { get; set; }
}
