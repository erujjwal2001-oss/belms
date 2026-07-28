using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IRepo;

public interface IWorkflowDefinitionRepository
{
    Task<WorkflowDefinition?> GetByIdWithStepsAsync(Guid id);

    IQueryable<WorkflowDefinition> Query();

    Task<WorkflowDefinition?> GetByNameAsync(string name);

    Task<WorkflowDefinition?> GetDefaultActiveAsync();

    Task<bool> ExistsByNameAsync(string name);

    Task AddAsync(WorkflowDefinition definition);

    Task UpdateAsync(WorkflowDefinition definition);

    Task DeleteAsync(WorkflowDefinition definition);

    Task SaveChangesAsync();
}
