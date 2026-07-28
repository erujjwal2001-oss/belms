namespace BELMS.Application.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalEmployees { get; set; }

    public int TotalAssets { get; set; }

    public int AssignedAssets { get; set; }

    public int PendingWorkflows { get; set; }

    public int CompletedWorkflows { get; set; }

    public int RejectedWorkflows { get; set; }

    public int PendingAccessRequests { get; set; }
}
