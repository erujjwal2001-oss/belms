using BELMS.Frontend.Features.Dashboard.Employee.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.Employee.Services;

public sealed class StaticEmployeeDashboardService : IEmployeeDashboardService
{
    public Task<EmployeeDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new EmployeeDashboardModel
        {
            WelcomeMessage = "Track your requests, leave balance, and assigned assets.",
            Stats =
            [
                new StatItem("Leave Balance", "12 days", DashboardIcons.Beach),
                new StatItem("Open Requests", "2", DashboardIcons.Assignment),
                new StatItem("Assigned Assets", "3", DashboardIcons.Devices),
                new StatItem("Pending Approvals", "1", DashboardIcons.Hourglass)
            ],
            PendingTasks =
            [
                new PendingTask("Annual leave — 3 days", "Pending", "HR Team", DateTime.Today.AddDays(5)),
                new PendingTask("Laptop upgrade request", "In Review", "IT Support", DateTime.Today.AddDays(2))
            ],
            RecentActivity =
            [
                new ActivityItem("Submitted leave request for Jul 10–12", "You", DateTime.Today.AddHours(-3), DashboardIcons.Event),
                new ActivityItem("Asset handover acknowledged", "IT Support", DateTime.Today.AddDays(-1), DashboardIcons.Devices),
                new ActivityItem("Profile update approved", "HR Team", DateTime.Today.AddDays(-2), DashboardIcons.Person)
            ],
            PrimaryChart = new ChartData(
                "Monthly Requests",
                ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                [1, 0, 2, 1, 3, 2]),
            QuickActions =
            [
                new QuickActionItem("Request Leave", DashboardIcons.Event),
                new QuickActionItem("View Payslips", DashboardIcons.Receipt),
                new QuickActionItem("Update Profile", DashboardIcons.Edit),
                new QuickActionItem("Report Issue", DashboardIcons.Bug)
            ],
            Notifications =
            [
                new NotificationItem("Leave approved", "Your leave for Jul 10–12 was approved.", DateTime.Today.AddHours(-2), false),
                new NotificationItem("Policy update", "Review the updated remote work policy.", DateTime.Today.AddDays(-1), true),
                new NotificationItem("Asset reminder", "Return spare keyboard by Friday.", DateTime.Today.AddDays(-2), true)
            ],
            WorkflowSteps =
            [
                new WorkflowStep("Submit Request", "Completed", true),
                new WorkflowStep("Manager Review", "In progress", false),
                new WorkflowStep("HR Approval", "Pending", false),
                new WorkflowStep("Finalized", "Pending", false)
            ]
        });
}
