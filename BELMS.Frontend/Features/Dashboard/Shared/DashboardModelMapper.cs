using BELMS.Frontend.Features.Dashboard.Shared.Models;
using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Features.Dashboard.Shared;

/// <summary>
/// Translates the server's <see cref="RoleDashboardDto"/> into the shared frontend view-model records.
/// </summary>
public static class DashboardModelMapper
{
    public static IReadOnlyList<StatItem> Stats(RoleDashboardDto dto) =>
        dto.Stats
            .Select(s => new StatItem(s.Title, s.Value, DashboardIconResolver.Resolve(s.Icon)))
            .ToList();

    public static IReadOnlyList<PendingTask> Tasks(RoleDashboardDto dto) =>
        dto.PendingTasks
            .Select(t => new PendingTask(t.Title, t.Status, t.Assignee, t.DueDate))
            .ToList();

    public static IReadOnlyList<ActivityItem> Activity(RoleDashboardDto dto) =>
        dto.RecentActivity
            .Select(a => new ActivityItem(a.Description, a.Actor, a.Timestamp, DashboardIconResolver.Resolve(a.Icon)))
            .ToList();

    public static IReadOnlyList<NotificationItem> Notifications(RoleDashboardDto dto) =>
        dto.Notifications
            .Select(n => new NotificationItem(n.Title, n.Message, n.Timestamp, n.IsUnread))
            .ToList();

    public static IReadOnlyList<QuickActionItem> QuickActions(RoleDashboardDto dto) =>
        dto.QuickActions
            .Select(q => new QuickActionItem(q.Label, DashboardIconResolver.Resolve(q.Icon), q.Href))
            .ToList();

    public static ChartData? Chart(DashboardChartDto? dto) =>
        dto is null ? null : new ChartData(dto.Title, dto.Labels, dto.Values);

    public static IReadOnlyList<WorkflowStep> WorkflowSteps(RoleDashboardDto dto) =>
        dto.PendingTasks
            .Select(t => new WorkflowStep(
                t.Title,
                t.Status,
                string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase)))
            .ToList();

    public static IReadOnlyList<AccessReviewItem> AccessReviews(RoleDashboardDto dto) =>
        dto.PendingTasks
            .Select(t => new AccessReviewItem(
                string.IsNullOrWhiteSpace(t.Assignee) ? "—" : t.Assignee,
                t.Title,
                t.Status,
                t.DueDate))
            .ToList();

    public static IReadOnlyList<AssetSummaryItem> AssetSummary(RoleDashboardDto dto)
    {
        var chart = dto.PrimaryChart;
        if (chart is null || chart.Values.Count == 0)
        {
            return [];
        }

        var total = (int)chart.Values.Sum();
        var availableIndex = chart.Labels.FindIndex(l => l.Contains("Available", StringComparison.OrdinalIgnoreCase));
        var available = availableIndex >= 0 && availableIndex < chart.Values.Count
            ? (int)chart.Values[availableIndex]
            : 0;

        return [new AssetSummaryItem("All Assets", total, available)];
    }
}
