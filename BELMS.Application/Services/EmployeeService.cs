using AutoMapper;
using AutoMapper.QueryableExtensions;
using BELMS.Application.Common.Pagination;
using BELMS.Application.DTOs.Employees;
using BELMS.Application.Filters;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using FluentValidation;

namespace BELMS.Application.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    IWorkflowDefinitionRepository workflowDefinitionRepository,
    IWorkflowInstanceService workflowInstanceService,
    IWorkflowRepository workflowRepository,
    IValidator<CreateEmployeeRequest> employeeValidator,
    IMapper mapper) : IEmployeeService
{
    public async Task<Result<EmployeeDto>> CreateAsync(CreateEmployeeRequest request)
    {
        var validationResult = await employeeValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var employeeCode = request.EmployeeCode.Trim();
        if (await employeeRepository.GetByEmployeeCodeAsync(employeeCode) is not null)
        {
            return Result<EmployeeDto>.Failure(
                Error.Conflict("Employee.CodeExists", ValidationMessages.EmployeeCodeAlreadyExists));
        }

        var employee = mapper.Map<Employee>(request);
        await employeeRepository.AddAsync(employee);
        await employeeRepository.SaveChangesAsync();

        var workflowDefinitionId = request.WorkflowDefinitionId;
        if (workflowDefinitionId is null)
        {
            var defaultDefinition = await workflowDefinitionRepository.GetDefaultActiveAsync();
            if (defaultDefinition is null)
            {
                return Result<EmployeeDto>.Failure(
                    Error.NotFound("WorkflowDefinition.NoActive", WorkflowDefinitionMessages.NoActiveDefinition));
            }

            workflowDefinitionId = defaultDefinition.Id;
        }

        var instanceResult = await workflowInstanceService.StartFromDefinitionAsync(employee.Id, workflowDefinitionId.Value);
        if (instanceResult.IsFailure)
        {
            return Result<EmployeeDto>.Failure(instanceResult.Error);
        }

        var dto = mapper.Map<EmployeeDto>(employee);
        dto.WorkflowInstanceId = instanceResult.Data;
        return Result<EmployeeDto>.Success(dto);
    }

    public async Task<Result<EmployeeDto>> GetByIdAsync(Guid id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return Result<EmployeeDto>.Failure(
                Error.NotFound("Employee.NotFound", EmployeeMessages.NotFound));
        }

        var dto = mapper.Map<EmployeeDto>(employee);
        var instance = await workflowRepository.GetLatestByEntityAsync(WorkflowEntityType.EmployeeOnboarding, id);
        if (instance is not null)
        {
            dto.WorkflowInstanceId = instance.Id;
        }

        return Result<EmployeeDto>.Success(dto);
    }

    public async Task<Result<PagedResponse<EmployeeDto>>> GetAllAsync(EmployeeFilterRequest request)
    {
        var query = employeeRepository.Query();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => x.FullName.Contains(request.Search) || x.Email.Contains(request.Search));
        }

        var dtoQuery = query.ProjectTo<EmployeeDto>(mapper.ConfigurationProvider);
        var result = await dtoQuery.ToPagedResponseAsync(request.PageNumber, request.PageSize);
        return Result<PagedResponse<EmployeeDto>>.Success(result);
    }

    public async Task<Result<EmployeeDto>> UpdateAsync(Guid id, UpdateEmployeeRequest request)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return Result<EmployeeDto>.Failure(
                Error.NotFound("Employee.NotFound", EmployeeMessages.NotFound));
        }

        if (!Enum.TryParse<EmployeeStatus>(request.Status, true, out var status))
        {
            return Result<EmployeeDto>.Failure(
                Error.Validation("Employee.InvalidStatus", ValidationMessages.ValidationFailed));
        }

        employee.FullName = request.FullName.Trim();
        employee.Department = request.Department.Trim();
        employee.Designation = request.Designation.Trim();
        employee.Status = status;

        await employeeRepository.UpdateAsync(employee);
        await employeeRepository.SaveChangesAsync();

        return Result<EmployeeDto>.Success(mapper.Map<EmployeeDto>(employee));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return Result.Failure(Error.NotFound("Employee.NotFound", EmployeeMessages.NotFound));
        }

        await employeeRepository.DeleteAsync(employee);
        await employeeRepository.SaveChangesAsync();
        return Result.Success();
    }
}
