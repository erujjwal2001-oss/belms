namespace BELMS.Application.DTOs.Dashboard;

/// <summary>
/// Rich, role-aware dashboard payload composed on the server so the frontend can render
/// an entire role dashboard from a single call.
/// </summary>
public class RoleDashboardDto
{
    public string Role { get; set; } = string.Empty;

    public string WelcomeMessage { get; set; } = string.Empty;

    public List<DashboardStatDto> Stats { get; set; } = [];

    public List<DashboardTaskDto> PendingTasks { get; set; } = [];

    public List<DashboardActivityDto> RecentActivity { get; set; } = [];

    public List<DashboardNotificationDto> Notifications { get; set; } = [];

    public DashboardChartDto? PrimaryChart { get; set; }

    public DashboardChartDto? SecondaryChart { get; set; }

    public List<DashboardQuickActionDto> QuickActions { get; set; } = [];
}

/// <summary>A KPI card. <see cref="Icon"/> is a semantic key resolved to an icon on the client.</summary>
public class DashboardStatDto
{
    public string Title { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
}

public class DashboardTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Assignee { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }
}

public class DashboardActivityDto
{
    public string Description { get; set; } = string.Empty;

    public string Actor { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public string Icon { get; set; } = string.Empty;
}

public class DashboardNotificationDto
{
    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public bool IsUnread { get; set; }
}

public class DashboardChartDto
{
    public string Title { get; set; } = string.Empty;

    public List<string> Labels { get; set; } = [];

    public List<double> Values { get; set; } = [];
}

public class DashboardQuickActionDto
{
    public string Label { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string? Href { get; set; }
}
