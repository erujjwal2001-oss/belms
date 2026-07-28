using BELMS.Domain.Common;

namespace BELMS.Domain.Entities;

public class WorkflowAttachment : BaseEntity
{
    public Guid WorkflowTaskId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public Guid UploadedByUserId { get; set; }

    public DateTime UploadedAt { get; set; }

    public WorkflowTask WorkflowTask { get; set; } = null!;

    public User UploadedByUser { get; set; } = null!;
}
