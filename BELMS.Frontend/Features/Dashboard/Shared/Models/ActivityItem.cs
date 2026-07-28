namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record ActivityItem(
    string Description,
    string Actor,
    DateTime Timestamp,
    string Icon);
