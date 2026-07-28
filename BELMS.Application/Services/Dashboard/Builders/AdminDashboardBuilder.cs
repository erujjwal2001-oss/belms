using BELMS.Application.DTOs.Dashboard;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class AdminDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "Admin";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot) => new()
    {
        Role = Role,
        WelcomeMessage = "System-wide overview, configuration, and governance.",
        Stats =
        [
            Stat("Employees", snapshot.TotalEmployees, DashboardIconKeys.People),
            Stat("Assets", snapshot.TotalAssets, DashboardIconKeys.Assets),
            Stat("Active Workflows", snapshot.InProgressWorkflows + snapshot.ReturnedWorkflows, DashboardIconKeys.Workflow),
            Stat("Pending Access", snapshot.PendingAccessRequests, DashboardIconKeys.Access)
        ],
        PendingTasks = TasksFromAccessRequests(snapshot),
        RecentActivity = ActivityFromNotifications(snapshot),
        Notifications = Notifications(snapshot),
        PrimaryChart = WorkflowChart(snapshot),
        SecondaryChart = new DashboardChartDto
        {
            Title = "Platform Inventory",
            Labels = ["Employees", "Assets", "Assigned", "Access"],
            Values =
            [
                snapshot.TotalEmployees,
                snapshot.TotalAssets,
                snapshot.AssignedAssets,
                snapshot.PendingAccessRequests
            ]
        },
        QuickActions =
        [
            new() { Label = "Manage Users", Icon = DashboardIconKeys.ManageAccounts, Href = "admin/users" },
            new() { Label = "Workflows", Icon = DashboardIconKeys.Workflow, Href = "admin/workflows" },
            new() { Label = "Settings", Icon = DashboardIconKeys.Settings, Href = "admin/settings" }
        ]
    };
}
