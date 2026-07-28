using BELMS.Domain.Common;

namespace BELMS.Domain.Entities;

public class EmployeeAsset : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public Guid AssetId { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public bool IsReturned { get; set; }

    public Employee Employee { get; set; } = null!;

    public Asset Asset { get; set; } = null!;
}