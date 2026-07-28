using BELMS.Application.DTOs.Workflows;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IWorkflowExecutionService
{
    Task<Result<WorkflowInstanceDto>> GetInstanceAsync(Guid instanceId);
    Task<Result<WorkflowTaskDto>> GetCurrentTaskAsync(Guid instanceId);
    Task<Result<List<WorkflowInstanceDto>>> GetPendingApprovalsAsync();
    Task<Result> ApproveCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request);
    Task<Result> RejectCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request);
    Task<Result> ReturnCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request);
    Task<Result> ResubmitCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request);
    Task<Result<WorkflowAttachmentDto>> UploadAttachmentAsync(
        Guid instanceId,
        Guid taskId,
        Stream fileStream,
        string fileName);
    Task<Result<WorkflowAttachmentDto>> GetAttachmentAsync(Guid attachmentId);
}
