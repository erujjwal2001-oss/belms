using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface IWorkflowApiClient
{
    // Definitions (Admin)
    Task<ApiResult<List<WorkflowDefinitionDto>>> GetDefinitionsAsync();

    Task<ApiResult<WorkflowDefinitionDto>> GetDefinitionAsync(Guid id);

    Task<ApiResult<WorkflowDefinitionDto>> CreateDefinitionAsync(CreateWorkflowDefinitionRequest request);

    Task<ApiResult<WorkflowDefinitionDto>> UpdateDefinitionAsync(Guid id, UpdateWorkflowDefinitionRequest request);

    Task<ApiResult> DeleteDefinitionAsync(Guid id);

    // Execution
    Task<ApiResult<List<WorkflowInstanceDto>>> GetPendingApprovalsAsync();

    Task<ApiResult<WorkflowInstanceDto>> GetInstanceAsync(Guid id);

    Task<ApiResult> ApproveAsync(Guid instanceId, CompleteTaskRequest request);

    Task<ApiResult> RejectAsync(Guid instanceId, CompleteTaskRequest request);

    Task<ApiResult> ReturnAsync(Guid instanceId, CompleteTaskRequest request);

    Task<ApiResult> ResubmitAsync(Guid instanceId, CompleteTaskRequest request);
}

public sealed class WorkflowApiClient(ApiHandler api) : ApiClientBase(api), IWorkflowApiClient
{
    public Task<ApiResult<List<WorkflowDefinitionDto>>> GetDefinitionsAsync() =>
        GetAsync<List<WorkflowDefinitionDto>>(ApiEndpoints.WorkflowDefinitions);

    public Task<ApiResult<WorkflowDefinitionDto>> GetDefinitionAsync(Guid id) =>
        GetAsync<WorkflowDefinitionDto>(ApiEndpoints.WorkflowDefinition(id));

    public Task<ApiResult<WorkflowDefinitionDto>> CreateDefinitionAsync(CreateWorkflowDefinitionRequest request) =>
        PostAsync<WorkflowDefinitionDto>(ApiEndpoints.WorkflowDefinitions, request);

    public Task<ApiResult<WorkflowDefinitionDto>> UpdateDefinitionAsync(Guid id, UpdateWorkflowDefinitionRequest request) =>
        PutAsync<WorkflowDefinitionDto>(ApiEndpoints.WorkflowDefinition(id), request);

    public Task<ApiResult> DeleteDefinitionAsync(Guid id) =>
        DeleteAsync(ApiEndpoints.WorkflowDefinition(id));

    public Task<ApiResult<List<WorkflowInstanceDto>>> GetPendingApprovalsAsync() =>
        GetAsync<List<WorkflowInstanceDto>>(ApiEndpoints.PendingApprovals);

    public Task<ApiResult<WorkflowInstanceDto>> GetInstanceAsync(Guid id) =>
        GetAsync<WorkflowInstanceDto>(ApiEndpoints.WorkflowInstance(id));

    public Task<ApiResult> ApproveAsync(Guid instanceId, CompleteTaskRequest request) =>
        PostAsync(ApiEndpoints.ApproveTask(instanceId), request);

    public Task<ApiResult> RejectAsync(Guid instanceId, CompleteTaskRequest request) =>
        PostAsync(ApiEndpoints.RejectTask(instanceId), request);

    public Task<ApiResult> ReturnAsync(Guid instanceId, CompleteTaskRequest request) =>
        PostAsync(ApiEndpoints.ReturnTask(instanceId), request);

    public Task<ApiResult> ResubmitAsync(Guid instanceId, CompleteTaskRequest request) =>
        PostAsync(ApiEndpoints.ResubmitTask(instanceId), request);
}
