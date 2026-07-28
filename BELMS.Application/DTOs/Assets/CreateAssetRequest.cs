using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Assets;

public class CreateAssetRequest
{
    [Required]
    public string AssetName { get; set; } = string.Empty;

    [Required]
    public string SerialNumber { get; set; } = string.Empty;

    [Required]
    public string AssetType { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;
}
