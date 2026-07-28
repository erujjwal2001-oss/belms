using BELMS.Domain.Common;
using BELMS.Domain.Enums;

namespace BELMS.Domain.Entities;

public class WorkflowStepDefinition : BaseEntity
{
    public Guid WorkflowDefinitionId { get; set; }

    public int StepOrder { get; set; }

    public string Name { get; set; } = string.Empty;

    public Role AssignedRole { get; set; }

    public string StepType { get; set; } = string.Empty;

    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
}
