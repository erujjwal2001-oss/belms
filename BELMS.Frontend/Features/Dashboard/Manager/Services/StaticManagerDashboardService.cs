using BELMS.Frontend.Features.Dashboard.Manager.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Manager.Services;

public sealed class StaticManagerDashboardService : IManagerDashboardService
{
    public Task<ManagerDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new ManagerDashboardModel
        {
            WelcomeMessage = "Monitor team performance and pending approvals.",
            Stats =
            [
                new StatItem("Team Members", "18", DashboardIcons.Groups),
                new StatItem("Pending Approvals", "4", DashboardIcons.Rule),
                new StatItem("On Leave Today", "2", DashboardIcons.EventBusy),
                new StatItem("Open Issues", "3", DashboardIcons.Flag)
            ],
            PendingTasks =
            [
                new PendingTask("Approve leave — Anisha Rai", "Pending", "You", DateTime.Today),
                new PendingTask("Expense claim — Bikash Lama", "Pending", "You", DateTime.Today.AddDays(1)),
                new PendingTask("Transfer request — Nisha KC", "In Review", "You", DateTime.Today.AddDays(2))
            ],
            RecentActivity =
            [
                new ActivityItem("Approved overtime for team member", "You", DateTime.Today.AddHours(-1), DashboardIcons.CheckCircle),
                new ActivityItem("Team standup notes published", "You", DateTime.Today.AddDays(-1), DashboardIcons.Notes),
                new ActivityItem("Performance review cycle opened", "HR Team", DateTime.Today.AddDays(-2), DashboardIcons.Star)
            ],
            PrimaryChart = new ChartData(
                "Team Attendance %",
                ["Mon", "Tue", "Wed", "Thu", "Fri"],
                [95, 92, 88, 94, 90]),
            QuickActions =
            [
                new QuickActionItem("Review Approvals", DashboardIcons.Rule),
                new QuickActionItem("Team Calendar", DashboardIcons.Calendar),
                new QuickActionItem("Assign Task", DashboardIcons.Assignment),
                new QuickActionItem("Team Report", DashboardIcons.Report)
            ],
            TeamMembers =
            [
                new TeamMember("Anisha Rai", "Senior Analyst", "Active", "AR"),
                new TeamMember("Bikash Lama", "Developer", "Remote", "BL"),
                new TeamMember("Nisha KC", "QA Engineer", "On Leave", "NK"),
                new TeamMember("Samir Thapa", "Business Analyst", "Active", "ST")
            ]
        });
}
