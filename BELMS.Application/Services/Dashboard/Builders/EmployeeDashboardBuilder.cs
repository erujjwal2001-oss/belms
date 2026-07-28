using BELMS.Application.DTOs.Dashboard;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class EmployeeDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "Employee";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot)
    {
        var myOpenRequests = snapshot.RecentAccessRequests
            .Count(r => r.Status is DomainTaskStatus.Pending or DomainTaskStatus.InProgress or DomainTaskStatus.Returned);

        return new RoleDashboardDto
        {
            Role = Role,
            WelcomeMessage = $"Welcome back, {snapshot.DisplayName}. Here is what needs your attention.",
            Stats =
            [
                Stat("My Requests", snapshot.RecentAccessRequests.Count, DashboardIconKeys.Access),
                Stat("Open Requests", myOpenRequests, DashboardIconKeys.Pending),
                Stat("Notifications", snapshot.Notifications.Count(n => !n.IsRead), DashboardIconKeys.Notifications),
                Stat("My Assets", snapshot.AssignedAssets, DashboardIconKeys.Assets)
            ],
            PendingTasks = TasksFromAccessRequests(snapshot),
            RecentActivity = ActivityFromNotifications(snapshot),
            Notifications = Notifications(snapshot),
            PrimaryChart = WorkflowChart(snapshot),
            QuickActions =
            [
                new() { Label = "My Requests", Icon = DashboardIconKeys.Access, Href = "employee/requests" },
                new() { Label = "My Assets", Icon = DashboardIconKeys.Assets, Href = "employee/assets" },
                new() { Label = "Notifications", Icon = DashboardIconKeys.Notifications, Href = "notifications" }
            ]
        };
    }
}
