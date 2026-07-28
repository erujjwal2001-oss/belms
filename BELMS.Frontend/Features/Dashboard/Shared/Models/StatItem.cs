namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record StatItem(
    string Title,
    string Value,
    string Icon,
    string? AccentColor = null);
