using BELMS.Domain.Entities;
using BELMS.Domain.Enums;

namespace BELMS.Application.Interfaces.IRepo;

public interface IWorkflowRepository
{
    Task<WorkflowInstance?> GetInstanceByIdAsync(Guid id);

    Task<WorkflowInstance?> GetInstanceWithTasksAsync(Guid id);

    Task<List<WorkflowInstance>> GetPendingApprovalsByRoleAsync(Role role);

    Task<List<WorkflowInstance>> GetAllInstancesWithTasksAsync();

    Task<WorkflowInstance?> GetLatestByEntityAsync(WorkflowEntityType entityType, Guid entityId);

    Task<WorkflowAttachment?> GetAttachmentByIdAsync(Guid id);

    Task AddInstanceAsync(WorkflowInstance instance);

    Task AddTaskAsync(WorkflowTask task);

    Task AddAttachmentAsync(WorkflowAttachment attachment);

    Task UpdateInstanceAsync(WorkflowInstance instance);

    Task UpdateTaskAsync(WorkflowTask task);

    Task<int> CountByStatusAsync(WorkflowInstanceStatus status);

    Task SaveChangesAsync();
}
