using BELMS.Domain.Common;

namespace BELMS.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string EntityName { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public Guid? PerformedByUserId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public User? PerformedByUser { get; set; }
}
