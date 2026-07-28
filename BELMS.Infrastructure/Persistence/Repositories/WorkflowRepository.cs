using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class WorkflowRepository(AppDbContext context) : IWorkflowRepository
{
    public async Task<WorkflowInstance?> GetInstanceWithTasksAsync(Guid id)
    {
        return await context.WorkflowInstances
            .Include(x => x.Tasks)
                .ThenInclude(t => t.Attachments)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<WorkflowInstance?> GetInstanceByIdAsync(Guid id)
    {
        return await context.WorkflowInstances
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<WorkflowInstance>> GetPendingApprovalsByRoleAsync(Role role)
    {
        return await context.WorkflowInstances
            .Include(x => x.Tasks)
            .Where(x => !x.IsDeleted
                && (x.Status == WorkflowInstanceStatus.InProgress || x.Status == WorkflowInstanceStatus.Returned)
                && x.Tasks.Any(t =>
                    t.StepOrder == x.CurrentStep
                    && t.AssignedRole == role
                    && t.Status == DomainTaskStatus.InProgress))
            .OrderByDescending(x => x.StartedAt)
            .ToListAsync();
    }

    public async Task<List<WorkflowInstance>> GetAllInstancesWithTasksAsync()
    {
        return await context.WorkflowInstances
            .Include(x => x.Tasks)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.StartedAt)
            .ToListAsync();
    }

    public async Task<WorkflowInstance?> GetLatestByEntityAsync(WorkflowEntityType entityType, Guid entityId)
    {
        return await context.WorkflowInstances
            .Where(x => !x.IsDeleted && x.EntityType == entityType && x.EntityId == entityId)
            .OrderByDescending(x => x.StartedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<WorkflowAttachment?> GetAttachmentByIdAsync(Guid id)
    {
        return await context.WorkflowAttachments
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddInstanceAsync(WorkflowInstance instance)
    {
        await context.WorkflowInstances.AddAsync(instance);
    }

    public async Task AddTaskAsync(WorkflowTask task)
    {
        await context.WorkflowTasks.AddAsync(task);
    }

    public async Task AddAttachmentAsync(WorkflowAttachment attachment)
    {
        await context.WorkflowAttachments.AddAsync(attachment);
    }

    public Task UpdateInstanceAsync(WorkflowInstance instance)
    {
        context.WorkflowInstances.Update(instance);
        return Task.CompletedTask;
    }

    public Task UpdateTaskAsync(WorkflowTask task)
    {
        context.WorkflowTasks.Update(task);
        return Task.CompletedTask;
    }

    public async Task<int> CountByStatusAsync(WorkflowInstanceStatus status)
    {
        return await context.WorkflowInstances
            .CountAsync(x => !x.IsDeleted && x.Status == status);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
