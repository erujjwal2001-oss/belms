using AutoMapper;
using BELMS.Application.DTOs.Workflows;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Application.Services;

public class WorkflowExecutionService(
    IWorkflowRepository workflowRepository,
    ICurrentUserService currentUserService,
    INotificationService notificationService,
    IAuditLogService auditLogService,
    IMapper mapper) : IWorkflowExecutionService
{
    public async Task<Result<WorkflowInstanceDto>> GetInstanceAsync(Guid instanceId)
    {
        // Load instance with all tasks and attachments
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result<WorkflowInstanceDto>.Failure(
                Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        return Result<WorkflowInstanceDto>.Success(mapper.Map<WorkflowInstanceDto>(instance));
    }

    public async Task<Result<WorkflowTaskDto>> GetCurrentTaskAsync(Guid instanceId)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result<WorkflowTaskDto>.Failure(
                Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        // Current task = task whose StepOrder matches instance.CurrentStep
        var currentTask = GetCurrentTask(instance);
        if (currentTask is null)
        {
            return Result<WorkflowTaskDto>.Failure(
                Error.NotFound("Workflow.NoCurrentTask", WorkflowMessages.NoCurrentTask));
        }

        return Result<WorkflowTaskDto>.Success(mapper.Map<WorkflowTaskDto>(currentTask));
    }

    public async Task<Result<List<WorkflowInstanceDto>>> GetPendingApprovalsAsync()
    {
        if (!currentUserService.IsAuthenticated || string.IsNullOrEmpty(currentUserService.Role))
        {
            return Result<List<WorkflowInstanceDto>>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        if (!Enum.TryParse<Role>(currentUserService.Role, true, out var role))
        {
            return Result<List<WorkflowInstanceDto>>.Failure(
                Error.Forbidden("Workflow.UnauthorizedRole", WorkflowMessages.UnauthorizedRole));
        }

        // Return instances where current step is InProgress and assigned to caller's role
        var instances = await workflowRepository.GetPendingApprovalsByRoleAsync(role);
        return Result<List<WorkflowInstanceDto>>.Success(mapper.Map<List<WorkflowInstanceDto>>(instances));
    }

    public async Task<Result> ApproveCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result.Failure(Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        // Approve allowed while running or after return correction at prior step
        if (instance.Status is not (WorkflowInstanceStatus.InProgress or WorkflowInstanceStatus.Returned))
        {
            return Result.Failure(Error.Validation("Workflow.NotInProgress", WorkflowMessages.NotInProgress));
        }

        var currentTask = GetCurrentTask(instance);
        if (currentTask is null || currentTask.Status != DomainTaskStatus.InProgress)
        {
            return Result.Failure(Error.NotFound("Workflow.NoCurrentTask", WorkflowMessages.NoCurrentTask));
        }

        // Only assigned role (or Admin) may act on this task
        var authResult = ValidateTaskRole(currentTask);
        if (authResult.IsFailure)
        {
            return authResult;
        }

        var userId = currentUserService.UserId!.Value;
        var now = DateTime.UtcNow;

        // Mark current task completed with actor and timestamp
        currentTask.Status = DomainTaskStatus.Completed;
        currentTask.Comments = request.Comments;
        currentTask.CompletedByUserId = userId;
        currentTask.CompletedAt = now;
        await workflowRepository.UpdateTaskAsync(currentTask);

        // Find next sequential step
        var nextTask = instance.Tasks
            .OrderBy(x => x.StepOrder)
            .FirstOrDefault(x => x.StepOrder > currentTask.StepOrder);

        if (nextTask is not null)
        {
            // Activate next step and notify its assignee role
            nextTask.Status = DomainTaskStatus.InProgress;
            instance.CurrentStep = nextTask.StepOrder;
            instance.Status = WorkflowInstanceStatus.InProgress;
            await workflowRepository.UpdateTaskAsync(nextTask);

            await notificationService.NotifyUsersByRoleAsync(
                nextTask.AssignedRole.ToString(),
                "New approval assigned",
                $"Workflow '{instance.WorkflowName}' requires your approval at step {nextTask.StepOrder}: {nextTask.StepName}.");
        }
        else
        {
            // Last step approved → workflow complete
            instance.Status = WorkflowInstanceStatus.Completed;
            instance.CompletedAt = now;
            await notificationService.NotifyUsersByRoleAsync(
                Role.HR.ToString(),
                "Workflow completed",
                $"Workflow '{instance.WorkflowName}' has been fully approved.");
        }

        await workflowRepository.UpdateInstanceAsync(instance);
        await workflowRepository.SaveChangesAsync();

        await auditLogService.LogAsync(
            nameof(WorkflowInstance),
            instance.Id,
            "Approve",
            userId,
            null,
            $"Step {currentTask.StepOrder} approved");

        return Result.Success();
    }

    public async Task<Result> RejectCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result.Failure(Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        // Reject only while actively in progress (not after return)
        if (instance.Status is not WorkflowInstanceStatus.InProgress)
        {
            return Result.Failure(Error.Validation("Workflow.NotInProgress", WorkflowMessages.NotInProgress));
        }

        var currentTask = GetCurrentTask(instance);
        if (currentTask is null || currentTask.Status != DomainTaskStatus.InProgress)
        {
            return Result.Failure(Error.NotFound("Workflow.NoCurrentTask", WorkflowMessages.NoCurrentTask));
        }

        var authResult = ValidateTaskRole(currentTask);
        if (authResult.IsFailure)
        {
            return authResult;
        }

        var userId = currentUserService.UserId!.Value;
        var now = DateTime.UtcNow;

        // Reject current task and terminate entire workflow
        currentTask.Status = DomainTaskStatus.Rejected;
        currentTask.Comments = request.Comments;
        currentTask.CompletedByUserId = userId;
        currentTask.CompletedAt = now;

        instance.Status = WorkflowInstanceStatus.Rejected;
        instance.CompletedAt = now;

        await workflowRepository.UpdateTaskAsync(currentTask);
        await workflowRepository.UpdateInstanceAsync(instance);
        await workflowRepository.SaveChangesAsync();

        await notificationService.NotifyUsersByRoleAsync(
            Role.HR.ToString(),
            "Workflow rejected",
            $"Workflow '{instance.WorkflowName}' was rejected at step {currentTask.StepOrder}: {currentTask.StepName}.");

        await auditLogService.LogAsync(
            nameof(WorkflowInstance),
            instance.Id,
            "Reject",
            userId,
            null,
            $"Step {currentTask.StepOrder} rejected");

        return Result.Success();
    }

    public async Task<Result> ReturnCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result.Failure(Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        if (instance.Status is not WorkflowInstanceStatus.InProgress)
        {
            return Result.Failure(Error.Validation("Workflow.NotInProgress", WorkflowMessages.NotInProgress));
        }

        var currentTask = GetCurrentTask(instance);
        if (currentTask is null || currentTask.Status != DomainTaskStatus.InProgress)
        {
            return Result.Failure(Error.NotFound("Workflow.NoCurrentTask", WorkflowMessages.NoCurrentTask));
        }

        // Cannot return from step 1 — nowhere to send back
        if (currentTask.StepOrder <= 1)
        {
            return Result.Failure(Error.Validation("Workflow.CannotReturn", WorkflowMessages.CannotReturnFirstStep));
        }

        var authResult = ValidateTaskRole(currentTask);
        if (authResult.IsFailure)
        {
            return authResult;
        }

        var userId = currentUserService.UserId!.Value;
        var now = DateTime.UtcNow;

        // Mark current step as Returned (keeps audit trail)
        currentTask.Status = DomainTaskStatus.Returned;
        currentTask.Comments = request.Comments;
        currentTask.CompletedByUserId = userId;
        currentTask.CompletedAt = now;

        // Reactivate previous step for correction — reset its completion data
        var previousTask = instance.Tasks
            .OrderByDescending(x => x.StepOrder)
            .First(x => x.StepOrder < currentTask.StepOrder);

        previousTask.Status = DomainTaskStatus.InProgress;
        previousTask.CompletedByUserId = null;
        previousTask.CompletedAt = null;
        previousTask.Comments = null;

        instance.CurrentStep = previousTask.StepOrder;
        instance.Status = WorkflowInstanceStatus.Returned;

        await workflowRepository.UpdateTaskAsync(currentTask);
        await workflowRepository.UpdateTaskAsync(previousTask);
        await workflowRepository.UpdateInstanceAsync(instance);
        await workflowRepository.SaveChangesAsync();

        await notificationService.NotifyUsersByRoleAsync(
            previousTask.AssignedRole.ToString(),
            "Workflow returned for correction",
            $"Workflow '{instance.WorkflowName}' was returned to step {previousTask.StepOrder}: {previousTask.StepName}.");

        await auditLogService.LogAsync(
            nameof(WorkflowInstance),
            instance.Id,
            "Return",
            userId,
            null,
            $"Step {currentTask.StepOrder} returned to step {previousTask.StepOrder}");

        return Result.Success();
    }

    public async Task<Result> ResubmitCurrentTaskAsync(Guid instanceId, CompleteTaskRequest request)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result.Failure(Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        // Resubmit only valid when workflow is in Returned state
        if (instance.Status is not WorkflowInstanceStatus.Returned)
        {
            return Result.Failure(Error.Validation("Workflow.CannotResubmit", WorkflowMessages.CannotResubmit));
        }

        var currentTask = GetCurrentTask(instance);
        if (currentTask is null || currentTask.Status != DomainTaskStatus.InProgress)
        {
            return Result.Failure(Error.NotFound("Workflow.NoCurrentTask", WorkflowMessages.NoCurrentTask));
        }

        var authResult = ValidateTaskRole(currentTask);
        if (authResult.IsFailure)
        {
            return authResult;
        }

        // Find the step that was Returned (must be ahead of current correction step)
        var returnedTask = instance.Tasks
            .FirstOrDefault(x => x.Status == DomainTaskStatus.Returned && x.StepOrder > currentTask.StepOrder);

        if (returnedTask is null)
        {
            return Result.Failure(Error.Validation("Workflow.CannotResubmit", WorkflowMessages.CannotResubmit));
        }

        var userId = currentUserService.UserId!.Value;
        var now = DateTime.UtcNow;

        // Complete correction step
        currentTask.Status = DomainTaskStatus.Completed;
        currentTask.Comments = request.Comments;
        currentTask.CompletedByUserId = userId;
        currentTask.CompletedAt = now;

        // Reactivate the step that originally returned the workflow
        returnedTask.Status = DomainTaskStatus.InProgress;
        returnedTask.CompletedByUserId = null;
        returnedTask.CompletedAt = null;
        returnedTask.Comments = null;

        instance.CurrentStep = returnedTask.StepOrder;
        instance.Status = WorkflowInstanceStatus.InProgress;

        await workflowRepository.UpdateTaskAsync(currentTask);
        await workflowRepository.UpdateTaskAsync(returnedTask);
        await workflowRepository.UpdateInstanceAsync(instance);
        await workflowRepository.SaveChangesAsync();

        await notificationService.NotifyUsersByRoleAsync(
            returnedTask.AssignedRole.ToString(),
            "Workflow resubmitted",
            $"Workflow '{instance.WorkflowName}' was resubmitted and requires your approval at step {returnedTask.StepOrder}.");

        await auditLogService.LogAsync(
            nameof(WorkflowInstance),
            instance.Id,
            "Resubmit",
            userId,
            null,
            $"Resubmitted from step {currentTask.StepOrder} to step {returnedTask.StepOrder}");

        return Result.Success();
    }

    public async Task<Result<WorkflowAttachmentDto>> UploadAttachmentAsync(
        Guid instanceId,
        Guid taskId,
        Stream fileStream,
        string fileName)
    {
        var instance = await workflowRepository.GetInstanceWithTasksAsync(instanceId);
        if (instance is null)
        {
            return Result<WorkflowAttachmentDto>.Failure(
                Error.NotFound("Workflow.NotFound", WorkflowMessages.NotFound));
        }

        // Task must belong to this instance
        var task = instance.Tasks.FirstOrDefault(x => x.Id == taskId);
        if (task is null)
        {
            return Result<WorkflowAttachmentDto>.Failure(
                Error.NotFound("Workflow.TaskNotFound", WorkflowMessages.TaskNotFound));
        }

        if (!currentUserService.IsAuthenticated)
        {
            return Result<WorkflowAttachmentDto>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        // Sanitize filename and write to disk under instance/task folder
        var safeFileName = Path.GetFileName(fileName);
        var storageDir = Path.Combine("uploads", "workflows", instanceId.ToString(), taskId.ToString());
        Directory.CreateDirectory(storageDir);
        var storedPath = Path.Combine(storageDir, $"{Guid.NewGuid()}_{safeFileName}");

        await using (var fs = File.Create(storedPath))
        {
            await fileStream.CopyToAsync(fs);
        }

        // Persist attachment metadata in database
        var attachment = new WorkflowAttachment
        {
            WorkflowTaskId = taskId,
            FileName = safeFileName,
            FilePath = storedPath.Replace('\\', '/'),
            UploadedByUserId = currentUserService.UserId!.Value,
            UploadedAt = DateTime.UtcNow
        };

        await workflowRepository.AddAttachmentAsync(attachment);
        await workflowRepository.SaveChangesAsync();

        return Result<WorkflowAttachmentDto>.Success(mapper.Map<WorkflowAttachmentDto>(attachment));
    }

    public async Task<Result<WorkflowAttachmentDto>> GetAttachmentAsync(Guid attachmentId)
    {
        var attachment = await workflowRepository.GetAttachmentByIdAsync(attachmentId);
        if (attachment is null)
        {
            return Result<WorkflowAttachmentDto>.Failure(
                Error.NotFound("Workflow.AttachmentNotFound", WorkflowMessages.AttachmentNotFound));
        }

        return Result<WorkflowAttachmentDto>.Success(mapper.Map<WorkflowAttachmentDto>(attachment));
    }

    // Resolves active task by matching StepOrder to instance.CurrentStep
    private static WorkflowTask? GetCurrentTask(WorkflowInstance instance)
    {
        return instance.Tasks
            .OrderBy(x => x.StepOrder)
            .FirstOrDefault(x => x.StepOrder == instance.CurrentStep);
    }

    // Admin may act on any task; others must match AssignedRole
    private Result ValidateTaskRole(WorkflowTask task)
    {
        if (!currentUserService.IsAuthenticated || string.IsNullOrEmpty(currentUserService.Role))
        {
            return Result.Failure(Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        if (!Enum.TryParse<Role>(currentUserService.Role, true, out var userRole))
        {
            return Result.Failure(Error.Forbidden("Workflow.UnauthorizedRole", WorkflowMessages.UnauthorizedRole));
        }

        if (userRole != Role.Admin && userRole != task.AssignedRole)
        {
            return Result.Failure(Error.Forbidden("Workflow.UnauthorizedRole", WorkflowMessages.UnauthorizedRole));
        }

        return Result.Success();
    }
}
