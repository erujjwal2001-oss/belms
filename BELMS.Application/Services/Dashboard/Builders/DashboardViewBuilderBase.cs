using BELMS.Application.DTOs.Dashboard;
using BELMS.Application.Interfaces.IService;

namespace BELMS.Application.Services.Dashboard.Builders;

/// <summary>
/// Shared helpers for turning a <see cref="DashboardSnapshot"/> into common dashboard sections.
/// </summary>
public abstract class DashboardViewBuilderBase : IDashboardViewBuilder
{
    public abstract string Role { get; }

    public abstract RoleDashboardDto Build(DashboardSnapshot snapshot);

    protected static DashboardStatDto Stat(string title, int value, string icon) =>
        new() { Title = title, Value = value.ToString(), Icon = icon };

    protected static DashboardStatDto Stat(string title, string value, string icon) =>
        new() { Title = title, Value = value, Icon = icon };

    /// <summary>Maps the current user's unread notifications into activity entries.</summary>
    protected static List<DashboardActivityDto> ActivityFromNotifications(DashboardSnapshot snapshot, int take = 6) =>
        snapshot.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .Select(n => new DashboardActivityDto
            {
                Description = n.Title,
                Actor = "System",
                Timestamp = n.CreatedAt,
                Icon = DashboardIconKeys.Notifications
            })
            .ToList();

    protected static List<DashboardNotificationDto> Notifications(DashboardSnapshot snapshot, int take = 8) =>
        snapshot.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .Select(n => new DashboardNotificationDto
            {
                Title = n.Title,
                Message = n.Message,
                Timestamp = n.CreatedAt,
                IsUnread = !n.IsRead
            })
            .ToList();

    /// <summary>Builds pending-task rows from the workflow approvals awaiting this role.</summary>
    protected static List<DashboardTaskDto> TasksFromApprovals(DashboardSnapshot snapshot, int take = 8)
    {
        var tasks = new List<DashboardTaskDto>();

        foreach (var instance in snapshot.PendingApprovals.Take(take))
        {
            var current = instance.Tasks?
                .OrderBy(t => t.StepOrder)
                .FirstOrDefault(t => t.StepOrder == instance.CurrentStep);

            tasks.Add(new DashboardTaskDto
            {
                Title = string.IsNullOrWhiteSpace(instance.WorkflowName)
                    ? "Workflow approval"
                    : instance.WorkflowName,
                Status = current?.Status.ToString() ?? instance.Status.ToString(),
                Assignee = current?.AssignedRole.ToString() ?? string.Empty,
                DueDate = instance.StartedAt
            });
        }

        return tasks;
    }

    protected static List<DashboardTaskDto> TasksFromAccessRequests(DashboardSnapshot snapshot, int take = 8) =>
        snapshot.RecentAccessRequests
            .Take(take)
            .Select(r => new DashboardTaskDto
            {
                Title = $"{r.RequestType} — {r.Employee?.FullName ?? "Employee"}",
                Status = r.Status.ToString(),
                Assignee = r.RequestedByUser?.FullName ?? string.Empty,
                DueDate = r.CreatedAt
            })
            .ToList();

    protected static DashboardChartDto WorkflowChart(DashboardSnapshot snapshot) => new()
    {
        Title = "Workflow Overview",
        Labels = ["In Progress", "Returned", "Completed", "Rejected"],
        Values =
        [
            snapshot.InProgressWorkflows,
            snapshot.ReturnedWorkflows,
            snapshot.CompletedWorkflows,
            snapshot.RejectedWorkflows
        ]
    };
}
