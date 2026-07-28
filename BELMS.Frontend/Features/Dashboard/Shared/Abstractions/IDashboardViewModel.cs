using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Shared.Abstractions;

public interface IDashboardViewModel
{
    string WelcomeMessage { get; }

    IReadOnlyList<StatItem> Stats { get; }

    IReadOnlyList<PendingTask> PendingTasks { get; }

    IReadOnlyList<ActivityItem> RecentActivity { get; }

    ChartData? PrimaryChart { get; }

    IReadOnlyList<QuickActionItem> QuickActions { get; }
}
