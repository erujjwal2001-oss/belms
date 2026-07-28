using BELMS.Domain.Common;
using BELMS.Domain.Enums;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Domain.Entities;

public class AccessRequest : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public string RequestType { get; set; } = string.Empty;

    public DomainTaskStatus Status { get; set; } = DomainTaskStatus.Pending;

    public Guid RequestedByUserId { get; set; }

    public Guid? ApprovedByUserId { get; set; }

    public string? Notes { get; set; }

    public Employee Employee { get; set; } = null!;

    public User RequestedByUser { get; set; } = null!;

    public User? ApprovedByUser { get; set; }
}
