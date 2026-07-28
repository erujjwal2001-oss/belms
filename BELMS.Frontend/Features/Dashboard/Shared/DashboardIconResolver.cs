namespace BELMS.Frontend.Features.Dashboard.Shared;

/// <summary>
/// Maps the semantic icon keys returned by the API to concrete MudBlazor icons,
/// keeping presentation concerns on the client.
/// </summary>
public static class DashboardIconResolver
{
    private static readonly IReadOnlyDictionary<string, string> Map =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["people"] = DashboardIcons.People,
            ["person-add"] = DashboardIcons.PersonAdd,
            ["assets"] = DashboardIcons.Devices,
            ["assigned"] = DashboardIcons.Computer,
            ["available"] = DashboardIcons.CheckCircle,
            ["workflow"] = DashboardIcons.AccountTree,
            ["pending"] = DashboardIcons.Hourglass,
            ["completed"] = DashboardIcons.CheckCircle,
            ["rejected"] = DashboardIcons.Flag,
            ["access"] = DashboardIcons.Key,
            ["notifications"] = DashboardIcons.Notifications,
            ["health"] = DashboardIcons.MonitorHeart,
            ["settings"] = DashboardIcons.Settings,
            ["approvals"] = DashboardIcons.Rule,
            ["report"] = DashboardIcons.Report,
            ["onboarding"] = DashboardIcons.PersonAdd,
            ["storage"] = DashboardIcons.Storage,
            ["cloud"] = DashboardIcons.CloudDone,
            ["security"] = DashboardIcons.Shield,
            ["check"] = DashboardIcons.Check,
            ["warning"] = DashboardIcons.Warning,
            ["ticket"] = DashboardIcons.ConfirmationNumber,
            ["history"] = DashboardIcons.History,
            ["manage-accounts"] = DashboardIcons.ManageAccounts
        };

    public static string Resolve(string? key)
    {
        if (!string.IsNullOrWhiteSpace(key) && Map.TryGetValue(key, out var icon))
        {
            return icon;
        }

        return DashboardIcons.ListAlt;
    }
}
