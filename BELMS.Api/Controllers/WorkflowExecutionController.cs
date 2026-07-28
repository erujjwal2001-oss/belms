using BELMS.Application.DTOs.Workflows;
using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/workflows/instances")]
[Authorize]
public class WorkflowExecutionController(IWorkflowExecutionService workflowExecutionService) : ApiControllerBase
{
    [HttpGet("pending-approvals")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        var result = await workflowExecutionService.GetPendingApprovalsAsync();
        return ProcessResult(result);
    }

    [HttpGet("{instanceId:guid}")]
    public async Task<IActionResult> GetInstance(Guid instanceId)
    {
        var result = await workflowExecutionService.GetInstanceAsync(instanceId);
        return ProcessResult(result);
    }

    [HttpGet("{instanceId:guid}/current-task")]
    public async Task<IActionResult> GetCurrentTask(Guid instanceId)
    {
        var result = await workflowExecutionService.GetCurrentTaskAsync(instanceId);
        return ProcessResult(result);
    }

    [HttpPost("{instanceId:guid}/approve")]
    public async Task<IActionResult> Approve(Guid instanceId, [FromBody] CompleteTaskRequest request)
    {
        var result = await workflowExecutionService.ApproveCurrentTaskAsync(instanceId, request);
        return ProcessResult(result);
    }

    [HttpPost("{instanceId:guid}/reject")]
    public async Task<IActionResult> Reject(Guid instanceId, [FromBody] CompleteTaskRequest request)
    {
        var result = await workflowExecutionService.RejectCurrentTaskAsync(instanceId, request);
        return ProcessResult(result);
    }

    [HttpPost("{instanceId:guid}/return")]
    public async Task<IActionResult> Return(Guid instanceId, [FromBody] CompleteTaskRequest request)
    {
        var result = await workflowExecutionService.ReturnCurrentTaskAsync(instanceId, request);
        return ProcessResult(result);
    }

    [HttpPost("{instanceId:guid}/resubmit")]
    public async Task<IActionResult> Resubmit(Guid instanceId, [FromBody] CompleteTaskRequest request)
    {
        var result = await workflowExecutionService.ResubmitCurrentTaskAsync(instanceId, request);
        return ProcessResult(result);
    }

    [HttpPost("{instanceId:guid}/tasks/{taskId:guid}/attachments")]
    public async Task<IActionResult> UploadAttachment(
        Guid instanceId,
        Guid taskId,
        IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        await using var stream = file.OpenReadStream();
        var result = await workflowExecutionService.UploadAttachmentAsync(
            instanceId,
            taskId,
            stream,
            file.FileName);

        return ProcessResult(result);
    }

    [HttpGet("attachments/{attachmentId:guid}")]
    public async Task<IActionResult> GetAttachment(Guid attachmentId)
    {
        var result = await workflowExecutionService.GetAttachmentAsync(attachmentId);
        if (result.IsFailure)
        {
            return ProcessResult(result);
        }

        var attachment = result.Data!;
        if (!System.IO.File.Exists(attachment.FilePath))
        {
            return NotFound("File not found on disk.");
        }

        var contentType = "application/octet-stream";
        return PhysicalFile(attachment.FilePath, contentType, attachment.FileName);
    }
}
