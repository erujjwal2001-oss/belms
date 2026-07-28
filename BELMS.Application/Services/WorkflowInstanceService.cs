using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Application.Services;

public class WorkflowInstanceService(
    IWorkflowDefinitionRepository workflowDefinitionRepository,
    IWorkflowRepository workflowRepository,
    INotificationService notificationService) : IWorkflowInstanceService
{
    public async Task<Result<Guid>> StartFromDefinitionAsync(Guid employeeId, Guid workflowDefinitionId)
    {
        // Load template with all step definitions
        var definition = await workflowDefinitionRepository.GetByIdWithStepsAsync(workflowDefinitionId);
        if (definition is null || !definition.IsActive)
        {
            return Result<Guid>.Failure(
                Error.NotFound("WorkflowDefinition.NotFound", WorkflowDefinitionMessages.NotFound));
        }

        var orderedSteps = definition.Steps.OrderBy(x => x.StepOrder).ToList();
        if (orderedSteps.Count == 0)
        {
            return Result<Guid>.Failure(
                Error.Validation("WorkflowDefinition.NoSteps", WorkflowDefinitionMessages.StepsRequired));
        }

        // Create runtime instance bound to EmployeeOnboarding entity
        var now = DateTime.UtcNow;
        var instance = new WorkflowInstance
        {
            EntityType = WorkflowEntityType.EmployeeOnboarding,
            EntityId = employeeId,
            EmployeeId = employeeId,
            WorkflowDefinitionId = definition.Id,
            WorkflowName = definition.Name,
            CurrentStep = 1,
            Status = WorkflowInstanceStatus.InProgress,
            StartedAt = now
        };

        await workflowRepository.AddInstanceAsync(instance);

        // Materialize one WorkflowTask per template step
        // Step 1 = InProgress, all others = Pending (sequential engine)
        foreach (var step in orderedSteps)
        {
            var task = new WorkflowTask
            {
                WorkflowInstanceId = instance.Id,
                StepOrder = step.StepOrder,
                StepName = step.Name,
                StepType = step.StepType,
                AssignedRole = step.AssignedRole,
                Status = step.StepOrder == 1 ? DomainTaskStatus.InProgress : DomainTaskStatus.Pending
            };

            await workflowRepository.AddTaskAsync(task);
        }

        await workflowRepository.SaveChangesAsync();

        // Notify users in the first step's assigned role
        var firstStep = orderedSteps[0];
        await notificationService.NotifyUsersByRoleAsync(
            firstStep.AssignedRole.ToString(),
            "New approval assigned",
            $"Employee onboarding workflow '{definition.Name}' requires your approval at step 1: {firstStep.Name}.");

        return Result<Guid>.Success(instance.Id);
    }
}
