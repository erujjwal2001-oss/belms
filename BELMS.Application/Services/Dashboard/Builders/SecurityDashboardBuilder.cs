using BELMS.Application.DTOs.Dashboard;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class SecurityDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "Security";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot) => new()
    {
        Role = Role,
        WelcomeMessage = "Access governance, reviews, and security approvals.",
        Stats =
        [
            Stat("Pending Access", snapshot.PendingAccessRequests, DashboardIconKeys.Access),
            Stat("Pending Approvals", snapshot.PendingApprovalsForMe, DashboardIconKeys.Approvals),
            Stat("Completed", snapshot.CompletedWorkflows, DashboardIconKeys.Completed),
            Stat("Rejected", snapshot.RejectedWorkflows, DashboardIconKeys.Rejected)
        ],
        PendingTasks = TasksFromAccessRequests(snapshot),
        RecentActivity = ActivityFromNotifications(snapshot),
        Notifications = Notifications(snapshot),
        PrimaryChart = WorkflowChart(snapshot),
        QuickActions =
        [
            new() { Label = "Access Requests", Icon = DashboardIconKeys.Access, Href = "security/access" },
            new() { Label = "Audit Export", Icon = DashboardIconKeys.History, Href = "security/audits" }
        ]
    };
}
