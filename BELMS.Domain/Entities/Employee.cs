using BELMS.Domain.Common;
using BELMS.Domain.Enums;

namespace BELMS.Domain.Entities;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public EmployeeStatus Status { get; set; } = EmployeeStatus.Pending;

    public ICollection<EmployeeAsset> EmployeeAssets { get; set; }
        = new List<EmployeeAsset>();
}