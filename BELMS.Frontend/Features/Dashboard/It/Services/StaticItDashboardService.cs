using BELMS.Frontend.Features.Dashboard.It.Models;
using BELMS.Frontend.Features.Dashboard.Shared;
using BELMS.Frontend.Features.Dashboard.Shared.Models;

namespace BELMS.Frontend.Features.Dashboard.It.Services;

public sealed class StaticItDashboardService : IItDashboardService
{
    public Task<ItDashboardModel> GetDashboardAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(new ItDashboardModel
        {
            WelcomeMessage = "Track assets, provisioning, and support tickets.",
            Stats =
            [
                new StatItem("Total Assets", "342", DashboardIcons.Devices),
                new StatItem("Assigned", "287", DashboardIcons.Link),
                new StatItem("Open Tickets", "11", DashboardIcons.Support),
                new StatItem("Provisioning", "6", DashboardIcons.CloudUpload)
            ],
            PendingTasks =
            [
                new PendingTask("Provision laptop — EMP-1042", "Pending", "IT Team", DateTime.Today),
                new PendingTask("VPN access — EMP-0987", "In Review", "IT Team", DateTime.Today.AddDays(1)),
                new PendingTask("Asset return — EMP-0765", "Pending", "IT Team", DateTime.Today.AddDays(2))
            ],
            RecentActivity =
            [
                new ActivityItem("Deployed 3 new workstations", "IT Team", DateTime.Today.AddHours(-4), DashboardIcons.Computer),
                new ActivityItem("Closed ticket #IT-4421", "Support", DateTime.Today.AddDays(-1), DashboardIcons.Check),
                new ActivityItem("Software license renewed", "System", DateTime.Today.AddDays(-2), DashboardIcons.Key)
            ],
            PrimaryChart = new ChartData(
                "Tickets Resolved",
                ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                [22, 18, 25, 30, 28, 32]),
            QuickActions =
            [
                new QuickActionItem("Assign Asset", DashboardIcons.Devices),
                new QuickActionItem("Open Ticket", DashboardIcons.ConfirmationNumber),
                new QuickActionItem("Provision Access", DashboardIcons.VpnKey),
                new QuickActionItem("Inventory Report", DashboardIcons.Inventory)
            ],
            AssetSummary =
            [
                new AssetSummaryItem("Laptops", 142, 18),
                new AssetSummaryItem("Desktops", 86, 9),
                new AssetSummaryItem("Monitors", 114, 22),
                new AssetSummaryItem("Mobile Devices", 48, 6)
            ]
        });
}
