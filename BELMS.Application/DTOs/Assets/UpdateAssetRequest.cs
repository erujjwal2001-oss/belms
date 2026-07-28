using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Assets;

public class UpdateAssetRequest
{
    [Required]
    public string AssetName { get; set; } = string.Empty;

    [Required]
    public string AssetType { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }
}
