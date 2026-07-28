namespace BELMS.Domain.Common.Constants;

public static class WorkflowDefinitionMessages
{
    public const string NotFound = "Workflow definition was not found.";
    public const string NameAlreadyExists = "Workflow definition name already exists.";
    public const string NoActiveDefinition = "No active workflow definition is available.";
    public const string StepsRequired = "At least one workflow step is required.";
    public const string InvalidStepOrder = "Workflow step order must be sequential starting from 1.";
    public const string CreationFailed = "Failed to create workflow definition.";
}
