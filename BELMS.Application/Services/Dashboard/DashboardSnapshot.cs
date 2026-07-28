using BELMS.Domain.Entities;

namespace BELMS.Application.Services.Dashboard;

/// <summary>
/// Raw aggregated data captured once from the module repositories. Role-specific builders
/// shape this snapshot into a <see cref="DTOs.Dashboard.RoleDashboardDto"/>.
/// </summary>
public sealed class DashboardSnapshot
{
    public string DisplayName { get; init; } = "there";

    public int TotalEmployees { get; init; }

    public int PendingEmployees { get; init; }

    public int ActiveEmployees { get; init; }

    public int TotalAssets { get; init; }

    public int AssignedAssets { get; init; }

    public int AvailableAssets { get; init; }

    public int InProgressWorkflows { get; init; }

    public int ReturnedWorkflows { get; init; }

    public int CompletedWorkflows { get; init; }

    public int RejectedWorkflows { get; init; }

    public int PendingAccessRequests { get; init; }

    public int PendingApprovalsForMe { get; init; }

    public IReadOnlyList<Employee> RecentEmployees { get; init; } = [];

    public IReadOnlyList<AccessRequest> RecentAccessRequests { get; init; } = [];

    public IReadOnlyList<Notification> Notifications { get; init; } = [];

    public IReadOnlyList<WorkflowInstance> PendingApprovals { get; init; } = [];
}
