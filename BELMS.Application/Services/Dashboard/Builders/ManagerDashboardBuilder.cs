using BELMS.Application.DTOs.Dashboard;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class ManagerDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "Manager";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot) => new()
    {
        Role = Role,
        WelcomeMessage = "Team approvals, workflow progress, and oversight.",
        Stats =
        [
            Stat("Pending Approvals", snapshot.PendingApprovalsForMe, DashboardIconKeys.Approvals),
            Stat("In Progress", snapshot.InProgressWorkflows, DashboardIconKeys.Workflow),
            Stat("Completed", snapshot.CompletedWorkflows, DashboardIconKeys.Completed),
            Stat("Returned", snapshot.ReturnedWorkflows, DashboardIconKeys.Warning)
        ],
        PendingTasks = TasksFromApprovals(snapshot),
        RecentActivity = ActivityFromNotifications(snapshot),
        Notifications = Notifications(snapshot),
        PrimaryChart = WorkflowChart(snapshot),
        QuickActions =
        [
            new() { Label = "Approvals", Icon = DashboardIconKeys.Approvals, Href = "manager/approvals" },
            new() { Label = "My Team", Icon = DashboardIconKeys.People, Href = "manager/team" }
        ]
    };
}
