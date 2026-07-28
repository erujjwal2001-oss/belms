using AutoMapper;
using BELMS.Application.DTOs.AccessRequests;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Application.Services;

public class AccessRequestService(
    IAccessRequestRepository accessRequestRepository,
    IEmployeeRepository employeeRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IAccessRequestService
{
    public async Task<Result<AccessRequestDto>> CreateAsync(CreateAccessRequestRequest request)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result<AccessRequestDto>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee is null)
        {
            return Result<AccessRequestDto>.Failure(
                Error.NotFound("Employee.NotFound", EmployeeMessages.NotFound));
        }

        var accessRequest = new AccessRequest
        {
            EmployeeId = request.EmployeeId,
            RequestType = request.RequestType.Trim(),
            Status = DomainTaskStatus.Pending,
            RequestedByUserId = currentUserService.UserId.Value,
            Notes = request.Notes
        };

        await accessRequestRepository.AddAsync(accessRequest);
        await accessRequestRepository.SaveChangesAsync();

        return Result<AccessRequestDto>.Success(await MapToDtoAsync(accessRequest));
    }

    public async Task<Result<AccessRequestDto>> GetByIdAsync(Guid id)
    {
        var accessRequest = await accessRequestRepository.GetByIdAsync(id);
        if (accessRequest is null)
        {
            return Result<AccessRequestDto>.Failure(
                Error.NotFound("AccessRequest.NotFound", AccessRequestMessages.NotFound));
        }

        return Result<AccessRequestDto>.Success(await MapToDtoAsync(accessRequest));
    }

    public async Task<Result<List<AccessRequestDto>>> GetAllAsync()
    {
        var requests = accessRequestRepository.Query()
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        var dtos = new List<AccessRequestDto>();
        foreach (var request in requests)
        {
            dtos.Add(await MapToDtoAsync(request));
        }

        return Result<List<AccessRequestDto>>.Success(dtos);
    }

    public async Task<Result<List<AccessRequestDto>>> GetMyRequestsAsync()
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result<List<AccessRequestDto>>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        var requests = accessRequestRepository.Query()
            .Where(x => x.RequestedByUserId == currentUserService.UserId.Value)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        var dtos = new List<AccessRequestDto>();
        foreach (var request in requests)
        {
            dtos.Add(await MapToDtoAsync(request));
        }

        return Result<List<AccessRequestDto>>.Success(dtos);
    }

    public async Task<Result<AccessRequestDto>> UpdateAsync(Guid id, UpdateAccessRequestRequest request)
    {
        var accessRequest = await accessRequestRepository.GetByIdAsync(id);
        if (accessRequest is null)
        {
            return Result<AccessRequestDto>.Failure(
                Error.NotFound("AccessRequest.NotFound", AccessRequestMessages.NotFound));
        }

        if (!Enum.TryParse<DomainTaskStatus>(request.Status, true, out var status))
        {
            return Result<AccessRequestDto>.Failure(
                Error.Validation("AccessRequest.InvalidStatus", ValidationMessages.ValidationFailed));
        }

        accessRequest.Status = status;
        accessRequest.Notes = request.Notes;

        if (status == DomainTaskStatus.Completed && currentUserService.UserId is not null)
        {
            accessRequest.ApprovedByUserId = currentUserService.UserId;
        }

        await accessRequestRepository.UpdateAsync(accessRequest);
        await accessRequestRepository.SaveChangesAsync();

        return Result<AccessRequestDto>.Success(await MapToDtoAsync(accessRequest));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var accessRequest = await accessRequestRepository.GetByIdAsync(id);
        if (accessRequest is null)
        {
            return Result.Failure(
                Error.NotFound("AccessRequest.NotFound", AccessRequestMessages.NotFound));
        }

        await accessRequestRepository.DeleteAsync(accessRequest);
        await accessRequestRepository.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<AccessRequestDto> MapToDtoAsync(AccessRequest request)
    {
        var loaded = await accessRequestRepository.GetByIdAsync(request.Id) ?? request;
        var dto = mapper.Map<AccessRequestDto>(loaded);
        dto.EmployeeName = loaded.Employee?.FullName ?? string.Empty;
        dto.RequestedByName = loaded.RequestedByUser?.FullName ?? string.Empty;
        return dto;
    }
}
