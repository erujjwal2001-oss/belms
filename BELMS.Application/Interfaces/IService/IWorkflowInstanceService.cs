using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IWorkflowInstanceService
{
    Task<Result<Guid>> StartFromDefinitionAsync(Guid employeeId, Guid workflowDefinitionId);
}
