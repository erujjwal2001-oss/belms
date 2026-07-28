using System.ComponentModel.DataAnnotations;

namespace BELMS.Frontend.Infrastructure.Api.Contracts;

public sealed class AssetDto
{
    public Guid Id { get; set; }

    public string AssetName { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public string AssetType { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }
}

public sealed class CreateAssetRequest
{
    [Required(ErrorMessage = "Asset name is required.")]
    public string AssetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serial number is required.")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Asset type is required.")]
    public string AssetType { get; set; } = "Laptop";

    public bool IsAvailable { get; set; } = true;
}

public sealed class UpdateAssetRequest
{
    [Required(ErrorMessage = "Asset name is required.")]
    public string AssetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Asset type is required.")]
    public string AssetType { get; set; } = "Laptop";

    public bool IsAvailable { get; set; }
}

public static class AssetTypes
{
    public static readonly IReadOnlyList<string> All =
        ["Laptop", "Desktop", "Mobile", "Monitor", "AccessCard", "Other"];
}
