using BELMS.Application.DTOs.Dashboard;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class ItDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "IT";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot) => new()
    {
        Role = Role,
        WelcomeMessage = "Asset provisioning, allocation, and IT approvals.",
        Stats =
        [
            Stat("Total Assets", snapshot.TotalAssets, DashboardIconKeys.Assets),
            Stat("Assigned", snapshot.AssignedAssets, DashboardIconKeys.Assigned),
            Stat("Available", snapshot.AvailableAssets, DashboardIconKeys.Available),
            Stat("Pending Approvals", snapshot.PendingApprovalsForMe, DashboardIconKeys.Approvals)
        ],
        PendingTasks = TasksFromApprovals(snapshot),
        RecentActivity = ActivityFromNotifications(snapshot),
        Notifications = Notifications(snapshot),
        PrimaryChart = new DashboardChartDto
        {
            Title = "Asset Allocation",
            Labels = ["Assigned", "Available"],
            Values = [snapshot.AssignedAssets, snapshot.AvailableAssets]
        },
        QuickActions =
        [
            new() { Label = "Manage Assets", Icon = DashboardIconKeys.Assets, Href = "it/assets" },
            new() { Label = "Approvals", Icon = DashboardIconKeys.Approvals, Href = "manager/approvals" }
        ]
    };
}
