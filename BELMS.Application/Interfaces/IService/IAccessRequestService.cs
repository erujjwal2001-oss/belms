using BELMS.Application.DTOs.AccessRequests;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IAccessRequestService
{
    Task<Result<AccessRequestDto>> CreateAsync(CreateAccessRequestRequest request);
    Task<Result<AccessRequestDto>> GetByIdAsync(Guid id);
    Task<Result<List<AccessRequestDto>>> GetAllAsync();
    Task<Result<List<AccessRequestDto>>> GetMyRequestsAsync();
    Task<Result<AccessRequestDto>> UpdateAsync(Guid id, UpdateAccessRequestRequest request);
    Task<Result> DeleteAsync(Guid id);
}
