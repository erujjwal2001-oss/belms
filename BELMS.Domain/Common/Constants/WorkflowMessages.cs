namespace BELMS.Domain.Common.Constants;

public static class WorkflowMessages
{
    public const string NotFound = "Workflow was not found.";
    public const string AlreadyCompleted = "Workflow is already completed.";
    public const string InvalidTransition = "Invalid workflow transition.";
    public const string TaskNotFound = "Workflow task was not found.";
    public const string ApprovalFailed = "Workflow approval failed.";
    public const string InstanceCreated = "Workflow instance started successfully.";
    public const string RuntimeNotImplemented = "Workflow runtime execution is not yet implemented.";
    public const string NoCurrentTask = "No active workflow task found for the current step.";
    public const string UnauthorizedRole = "You are not authorized to act on this workflow task.";
    public const string NotInProgress = "Workflow is not in progress.";
    public const string CannotReturnFirstStep = "Cannot return workflow from the first step.";
    public const string CannotResubmit = "Workflow cannot be resubmitted in its current state.";
    public const string AttachmentNotFound = "Workflow attachment was not found.";
}
