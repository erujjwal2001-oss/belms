using BELMS.Frontend.Features.Dashboard.Hr.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Hr.Services;

public sealed class StaticHrDashboardService : IHrDashboardService
{
    public Task<HrDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new HrDashboardModel
        {
            WelcomeMessage = "Manage employees, onboarding, and lifecycle workflows.",
            Stats =
            [
                new StatItem("Total Employees", "128", DashboardIcons.Groups),
                new StatItem("New Joinees", "5", DashboardIcons.PersonAdd),
                new StatItem("Active Workdlflows", "14", DashboardIcons.EventBusy),
                new StatItem("Pending Onboards", "9", DashboardIcons.Timeline)

            ],
            PendingTasks =
            [
                new PendingTask("Onboard — Priya Sharma", "Pending", "HR Team", DateTime.Today.AddDays(1)),
                new PendingTask("Leave approval — Raj Karki", "Pending", "You", DateTime.Today),
                new PendingTask("Exit clearance — Samir Thapa", "In Review", "HR Team", DateTime.Today.AddDays(3))
            ],
            RecentActivity =
            [
                new ActivityItem("New hire record created", "HR Team", DateTime.Today.AddHours(-2), DashboardIcons.PersonAdd),
                new ActivityItem("Bulk employee import completed", "System", DateTime.Today.AddDays(-1), DashboardIcons.Upload),
                new ActivityItem("Policy acknowledgment sent", "HR Team", DateTime.Today.AddDays(-1), DashboardIcons.Mail)
            ],
            PrimaryChart = new ChartData(
                "Workflows Summary",
                ["Completed","In Progress","Pending"],
                [3,6,7]),
            QuickActions =
            [
                new QuickActionItem("Add Employee", DashboardIcons.PersonAdd),
                new QuickActionItem("Manage Leave", DashboardIcons.Event),
                new QuickActionItem("Export Reports", DashboardIcons.Download),
                new QuickActionItem("Workflow Queue", DashboardIcons.ListAlt)
            ]
        });
}
