using AutoMapper;
using BELMS.Application.DTOs.Workflows;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;

namespace BELMS.Application.Services;

public class WorkflowDefinitionService(
    IWorkflowDefinitionRepository workflowDefinitionRepository,
    IMapper mapper) : IWorkflowDefinitionService
{
    public async Task<Result<WorkflowDefinitionDto>> CreateAsync(CreateWorkflowDefinitionRequest request)
    {
        var name = request.Name.Trim();

        if (await workflowDefinitionRepository.ExistsByNameAsync(name))
        {
            return Result<WorkflowDefinitionDto>.Failure(
                Error.Conflict("WorkflowDefinition.NameExists", WorkflowDefinitionMessages.NameAlreadyExists));
        }

        if (!HasSequentialStepOrder(request.Steps))
        {
            return Result<WorkflowDefinitionDto>.Failure(
                Error.Validation("WorkflowDefinition.InvalidSteps", WorkflowDefinitionMessages.InvalidStepOrder));
        }

        foreach (var step in request.Steps)
        {
            if (!Enum.TryParse<Role>(step.AssignedRole, true, out _))
            {
                return Result<WorkflowDefinitionDto>.Failure(
                    Error.Validation("WorkflowDefinition.InvalidRole", ValidationMessages.ValidationFailed));
            }
        }

        var definition = mapper.Map<WorkflowDefinition>(request);
        definition.Name = name;
        definition.Description = request.Description.Trim();

        await workflowDefinitionRepository.AddAsync(definition);
        await workflowDefinitionRepository.SaveChangesAsync();

        return Result<WorkflowDefinitionDto>.Success(mapper.Map<WorkflowDefinitionDto>(definition));
    }

    public async Task<Result<List<WorkflowDefinitionDto>>> GetAllAsync()
    {
        var definitions = workflowDefinitionRepository.Query()
            .OrderBy(x => x.Name)
            .ToList();

        return Result<List<WorkflowDefinitionDto>>.Success(mapper.Map<List<WorkflowDefinitionDto>>(definitions));
    }

    public async Task<Result<WorkflowDefinitionDto>> GetByIdAsync(Guid id)
    {
        var definition = await workflowDefinitionRepository.GetByIdWithStepsAsync(id);
        if (definition is null)
        {
            return Result<WorkflowDefinitionDto>.Failure(
                Error.NotFound("WorkflowDefinition.NotFound", WorkflowDefinitionMessages.NotFound));
        }

        return Result<WorkflowDefinitionDto>.Success(mapper.Map<WorkflowDefinitionDto>(definition));
    }

    public async Task<Result<WorkflowDefinitionDto>> UpdateAsync(Guid id, UpdateWorkflowDefinitionRequest request)
    {
        var definition = await workflowDefinitionRepository.GetByIdWithStepsAsync(id);
        if (definition is null)
        {
            return Result<WorkflowDefinitionDto>.Failure(
                Error.NotFound("WorkflowDefinition.NotFound", WorkflowDefinitionMessages.NotFound));
        }

        var name = request.Name.Trim();
        var nameConflict = workflowDefinitionRepository.Query()
            .Any(x => x.Name == name && x.Id != id);
        if (nameConflict)
        {
            return Result<WorkflowDefinitionDto>.Failure(
                Error.Conflict("WorkflowDefinition.NameExists", WorkflowDefinitionMessages.NameAlreadyExists));
        }

        definition.Name = name;
        definition.Description = request.Description.Trim();
        definition.IsActive = request.IsActive;

        await workflowDefinitionRepository.UpdateAsync(definition);
        await workflowDefinitionRepository.SaveChangesAsync();

        return Result<WorkflowDefinitionDto>.Success(mapper.Map<WorkflowDefinitionDto>(definition));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var definition = await workflowDefinitionRepository.GetByIdWithStepsAsync(id);
        if (definition is null)
        {
            return Result.Failure(
                Error.NotFound("WorkflowDefinition.NotFound", WorkflowDefinitionMessages.NotFound));
        }

        await workflowDefinitionRepository.DeleteAsync(definition);
        await workflowDefinitionRepository.SaveChangesAsync();
        return Result.Success();
    }

    private static bool HasSequentialStepOrder(List<CreateWorkflowStepRequest> steps)
    {
        var ordered = steps.OrderBy(x => x.StepOrder).Select(x => x.StepOrder).ToList();
        for (var i = 0; i < ordered.Count; i++)
        {
            if (ordered[i] != i + 1)
            {
                return false;
            }
        }

        return true;
    }
}
