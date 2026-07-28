namespace BELMS.Application.DTOs.Assets;

public class AssetDto
{
    public Guid Id { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}
