using BELMS.Frontend.Features.Dashboard.Shared.Abstractions;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Admin.Models;

public sealed class AdminDashboardModel : IDashboardViewModel
{
    public string WelcomeMessage { get; init; } = string.Empty;
    public IReadOnlyList<StatItem> Stats { get; init; } = [];
    public IReadOnlyList<PendingTask> PendingTasks { get; init; } = [];
    public IReadOnlyList<ActivityItem> RecentActivity { get; init; } = [];
    public ChartData? PrimaryChart { get; init; }
    public ChartData? RegistrationsChart { get; init; }
    public IReadOnlyList<QuickActionItem> QuickActions { get; init; } = [];
}
