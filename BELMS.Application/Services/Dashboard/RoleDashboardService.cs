using BELMS.Application.DTOs.Dashboard;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Application.Services.Dashboard;

/// <summary>
/// Aggregates a single <see cref="DashboardSnapshot"/> from the module repositories and
/// delegates shaping to the role-specific builder resolved by the factory.
/// </summary>
public sealed class RoleDashboardService(
    IEmployeeRepository employeeRepository,
    IAssetRepository assetRepository,
    IWorkflowRepository workflowRepository,
    IAccessRequestRepository accessRequestRepository,
    INotificationRepository notificationRepository,
    ICurrentUserService currentUser,
    IDashboardBuilderFactory builderFactory) : IRoleDashboardService
{
    public async Task<Result<RoleDashboardDto>> GetOverviewAsync()
    {
        if (!currentUser.IsAuthenticated)
        {
            return Result<RoleDashboardDto>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        var role = currentUser.Role;

        var totalAssets = await assetRepository.CountAsync();
        var assignedAssets = await assetRepository.CountAssignedAsync();

        var employees = employeeRepository.Query();
        var pendingEmployees = await employees.CountAsync(e => e.Status == EmployeeStatus.Pending);
        var activeEmployees = await employees.CountAsync(e => e.Status == EmployeeStatus.Active);

        var recentEmployees = await employeeRepository.Query()
            .OrderByDescending(e => e.CreatedAt)
            .Take(8)
            .ToListAsync();

        var recentAccessRequests = await accessRequestRepository.Query()
            .OrderByDescending(r => r.CreatedAt)
            .Take(8)
            .ToListAsync();

        var notifications = currentUser.UserId is { } userId
            ? await notificationRepository.GetByUserIdAsync(userId)
            : [];

        var pendingApprovals = Enum.TryParse<Role>(role, true, out var roleEnum)
            ? await workflowRepository.GetPendingApprovalsByRoleAsync(roleEnum)
            : [];

        var snapshot = new DashboardSnapshot
        {
            DisplayName = ResolveDisplayName(),
            TotalEmployees = await employeeRepository.CountAsync(),
            PendingEmployees = pendingEmployees,
            ActiveEmployees = activeEmployees,
            TotalAssets = totalAssets,
            AssignedAssets = assignedAssets,
            AvailableAssets = Math.Max(0, totalAssets - assignedAssets),
            InProgressWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.InProgress),
            ReturnedWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Returned),
            CompletedWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Completed),
            RejectedWorkflows = await workflowRepository.CountByStatusAsync(WorkflowInstanceStatus.Rejected),
            PendingAccessRequests = await accessRequestRepository.CountPendingAsync(),
            PendingApprovalsForMe = pendingApprovals.Count,
            RecentEmployees = recentEmployees,
            RecentAccessRequests = recentAccessRequests,
            Notifications = notifications,
            PendingApprovals = pendingApprovals
        };

        var builder = builderFactory.Create(role);
        return Result<RoleDashboardDto>.Success(builder.Build(snapshot));
    }

    private string ResolveDisplayName()
    {
        var email = currentUser.Email;
        if (string.IsNullOrWhiteSpace(email))
        {
            return "there";
        }

        var local = email.Split('@')[0];
        return string.IsNullOrWhiteSpace(local) ? "there" : local;
    }
}
