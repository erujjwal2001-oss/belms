using BELMS.Application.DTOs.Dashboard;

namespace BELMS.Application.Services.Dashboard.Builders;

public sealed class HrDashboardBuilder : DashboardViewBuilderBase
{
    public override string Role => "HR";

    public override RoleDashboardDto Build(DashboardSnapshot snapshot)
    {
        var onboardingTasks = snapshot.RecentEmployees
            .Take(8)
            .Select(e => new DashboardTaskDto
            {
                Title = $"Onboarding — {e.FullName}",
                Status = e.Status.ToString(),
                Assignee = string.IsNullOrWhiteSpace(e.Department) ? "HR" : e.Department,
                DueDate = e.CreatedAt
            })
            .ToList();

        return new RoleDashboardDto
        {
            Role = Role,
            WelcomeMessage = "Employee lifecycle, onboarding, and people operations.",
            Stats =
            [
                Stat("Employees", snapshot.TotalEmployees, DashboardIconKeys.People),
                Stat("Pending Onboarding", snapshot.PendingEmployees, DashboardIconKeys.Onboarding),
                Stat("Active", snapshot.ActiveEmployees, DashboardIconKeys.Check),
                Stat("Access Requests", snapshot.PendingAccessRequests, DashboardIconKeys.Access)
            ],
            PendingTasks = onboardingTasks,
            RecentActivity = ActivityFromNotifications(snapshot),
            Notifications = Notifications(snapshot),
            PrimaryChart = new DashboardChartDto
            {
                Title = "Workforce Status",
                Labels = ["Active", "Pending", "Other"],
                Values =
                [
                    snapshot.ActiveEmployees,
                    snapshot.PendingEmployees,
                    Math.Max(0, snapshot.TotalEmployees - snapshot.ActiveEmployees - snapshot.PendingEmployees)
                ]
            },
            QuickActions =
            [
                new() { Label = "Manage Employees", Icon = DashboardIconKeys.People, Href = "hr/employees" },
                new() { Label = "Assets", Icon = DashboardIconKeys.Assets, Href = "hr/assets" },
                new() { Label = "Reports", Icon = DashboardIconKeys.Report, Href = "hr/reports" }
            ]
        };
    }
}
