using BELMS.Frontend.Features.Dashboard.Security.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Security.Services;

public sealed class StaticSecurityDashboardService : ISecurityDashboardService
{
    public Task<SecurityDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new SecurityDashboardModel
        {
            WelcomeMessage = "Review access requests, audits, and security incidents.",
            Stats =
            [
                new StatItem("Access Requests", "6", DashboardIcons.Lock),
                new StatItem("Active Audits", "2", DashboardIcons.FactCheck),
                new StatItem("Incidents", "1", DashboardIcons.Warning),
                new StatItem("Cleared Today", "8", DashboardIcons.Verified)
            ],
            PendingTasks =
            [
                new PendingTask("Core banking access — EMP-1102", "Pending", "Security", DateTime.Today),
                new PendingTask("Privileged role review", "In Review", "Security", DateTime.Today.AddDays(1)),
                new PendingTask("Badge deactivation — EMP-0654", "Pending", "Security", DateTime.Today)
            ],
            RecentActivity =
            [
                new ActivityItem("Access request approved for branch teller", "Security", DateTime.Today.AddHours(-2), DashboardIcons.LockOpen),
                new ActivityItem("Quarterly audit log exported", "System", DateTime.Today.AddDays(-1), DashboardIcons.Download),
                new ActivityItem("Suspicious login flagged and cleared", "Security", DateTime.Today.AddDays(-1), DashboardIcons.Shield)
            ],
            PrimaryChart = new ChartData(
                "Access Requests",
                ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                [12, 9, 14, 11, 8, 6]),
            QuickActions =
            [
                new QuickActionItem("Review Access", DashboardIcons.Lock),
                new QuickActionItem("Run Audit", DashboardIcons.FactCheck),
                new QuickActionItem("Incident Log", DashboardIcons.Report),
                new QuickActionItem("Policy Center", DashboardIcons.Policy)
            ],
            AccessReviews =
            [
                new AccessReviewItem("Raj Karki", "Core Banking", "Pending", DateTime.Today.AddDays(2)),
                new AccessReviewItem("Priya Sharma", "Admin Portal", "Approved", DateTime.Today.AddDays(-1)),
                new AccessReviewItem("Samir Thapa", "VPN Gateway", "Escalated", DateTime.Today)
            ]
        });
}
