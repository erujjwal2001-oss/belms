namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record QuickActionItem(
    string Label,
    string Icon,
    string? Href = null);
