using BELMS.Frontend.Features.Dashboard.Admin.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Admin.Services;

public sealed class StaticAdminDashboardService : IAdminDashboardService
{
    public Task<AdminDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new AdminDashboardModel
        {
            WelcomeMessage = "System-wide overview, configuration, and governance.",
            Stats =
            [
                new StatItem("Active Users", "128", DashboardIcons.People),
                new StatItem("Workflows", "106", DashboardIcons.AccountTree),
                new StatItem("System Health", "99.2%", DashboardIcons.MonitorHeart),
                new StatItem("Pending Config", "3", DashboardIcons.Settings)
            ],
            PendingTasks =
            [
                new PendingTask("Update workflow definition — Onboarding", "Pending", "Admin", DateTime.Today),
                new PendingTask("Review role permissions matrix", "In Review", "Admin", DateTime.Today.AddDays(2)),
                new PendingTask("Approve system maintenance window", "Pending", "Admin", DateTime.Today.AddDays(5))
            ],
            RecentActivity =
            [
                new ActivityItem("Seeded demo users refreshed", "System", DateTime.Today.AddHours(-6), DashboardIcons.Storage),
                new ActivityItem("Notification template updated", "Admin", DateTime.Today.AddDays(-1), DashboardIcons.Notifications),
                new ActivityItem("API health check passed", "System", DateTime.Today.AddDays(-1), DashboardIcons.CloudDone)
            ],
            PrimaryChart = new ChartData(
                "Platform Usage",
                ["HR", "IT", "Mgr", "Sec", "Emp", "Adm"],
                [42, 38, 35, 28, 95, 12]),
            RegistrationsChart = new ChartData(
                "User Registrations",
                ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                [8, 12, 10, 15, 11, 14]),
            QuickActions =
            [
                new QuickActionItem("Manage Users", DashboardIcons.ManageAccounts),
                new QuickActionItem("Workflow Config", DashboardIcons.AccountTree),
                new QuickActionItem("System Settings", DashboardIcons.Settings),
                new QuickActionItem("Audit Logs", DashboardIcons.History)
            ]
        });
}
