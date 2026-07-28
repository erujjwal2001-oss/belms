using BELMS.Domain.Common;

namespace BELMS.Domain.Entities;

public class WorkflowDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<WorkflowStepDefinition> Steps { get; set; } = new List<WorkflowStepDefinition>();
}
