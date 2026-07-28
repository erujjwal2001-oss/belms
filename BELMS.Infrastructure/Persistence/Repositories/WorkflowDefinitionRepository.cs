using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class WorkflowDefinitionRepository(AppDbContext context) : IWorkflowDefinitionRepository
{
    public async Task<WorkflowDefinition?> GetByIdWithStepsAsync(Guid id)
    {
        return await context.WorkflowDefinitions
            .Include(x => x.Steps)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public IQueryable<WorkflowDefinition> Query()
    {
        return context.WorkflowDefinitions
            .Include(x => x.Steps)
            .Where(x => !x.IsDeleted);
    }

    public async Task<WorkflowDefinition?> GetByNameAsync(string name)
    {
        return await context.WorkflowDefinitions
            .Include(x => x.Steps)
            .FirstOrDefaultAsync(x => x.Name == name && !x.IsDeleted && x.IsActive);
    }

    public async Task<WorkflowDefinition?> GetDefaultActiveAsync()
    {
        return await context.WorkflowDefinitions
            .Include(x => x.Steps)
            .Where(x => !x.IsDeleted && x.IsActive)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await context.WorkflowDefinitions
            .AnyAsync(x => x.Name == name && !x.IsDeleted);
    }

    public async Task AddAsync(WorkflowDefinition definition)
    {
        await context.WorkflowDefinitions.AddAsync(definition);
    }

    public Task UpdateAsync(WorkflowDefinition definition)
    {
        context.WorkflowDefinitions.Update(definition);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(WorkflowDefinition definition)
    {
        definition.IsDeleted = true;
        definition.IsActive = false;
        context.WorkflowDefinitions.Update(definition);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
