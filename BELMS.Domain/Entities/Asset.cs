using BELMS.Domain.Common;
using BELMS.Domain.Enums;

namespace BELMS.Domain.Entities;

public class Asset : BaseEntity
{
    public string AssetName { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public AssetType AssetType { get; set; }

    public bool IsAvailable { get; set; } = true;

    public ICollection<EmployeeAsset> EmployeeAssets { get; set; }
        = new List<EmployeeAsset>();
}