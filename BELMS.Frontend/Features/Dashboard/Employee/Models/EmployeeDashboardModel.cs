using BELMS.Frontend.Features.Dashboard.Shared.Abstractions;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Employee.Models;

public sealed class EmployeeDashboardModel : IDashboardViewModel
{
    public string WelcomeMessage { get; init; } = string.Empty;

    public IReadOnlyList<StatItem> Stats { get; init; } = [];

    public IReadOnlyList<PendingTask> PendingTasks { get; init; } = [];

    public IReadOnlyList<ActivityItem> RecentActivity { get; init; } = [];

    public ChartData? PrimaryChart { get; init; }

    public IReadOnlyList<QuickActionItem> QuickActions { get; init; } = [];

    public IReadOnlyList<NotificationItem> Notifications { get; init; } = [];

    public IReadOnlyList<WorkflowStep> WorkflowSteps { get; init; } = [];
}
