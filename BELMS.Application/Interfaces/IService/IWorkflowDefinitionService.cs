using BELMS.Application.DTOs.Workflows;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IWorkflowDefinitionService
{
    Task<Result<WorkflowDefinitionDto>> CreateAsync(CreateWorkflowDefinitionRequest request);
    Task<Result<List<WorkflowDefinitionDto>>> GetAllAsync();
    Task<Result<WorkflowDefinitionDto>> GetByIdAsync(Guid id);
    Task<Result<WorkflowDefinitionDto>> UpdateAsync(Guid id, UpdateWorkflowDefinitionRequest request);
    Task<Result> DeleteAsync(Guid id);
}
