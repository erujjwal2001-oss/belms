using BELMS.Application.DTOs.Dashboard;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Enums;

namespace BELMS.Application.Services;

public class DashboardService(
    IEmployeeRepository employeeRepository,
    IAssetRepository assetRepository,
    IWorkflowRepository workflowRepository,
    IAccessRequestRepository accessRequestRepository) : IDashboardService
{
    public async Task<Result<DashboardDto>> GetDashboardAsync()
    {
        // Aggregate counts from each module repository into a single KPI DTO
        var dashboard = new DashboardDto
        {
            TotalEmployees = await employeeRepository.CountAsync(),
            TotalAssets = await assetRepository.CountAsync(),
            AssignedAssets = await assetRepository.CountAssignedAsync(),

            // Pending = actively running + returned for correction
            PendingWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.InProgress)
                + await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Returned),

            CompletedWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Completed),
            RejectedWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Rejected),
            PendingAccessRequests = await accessRequestRepository.CountPendingAsync()
        };

        return Result<DashboardDto>.Success(dashboard);
    }
}
