using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface IAccessRequestApiClient
{
    Task<ApiResult<List<AccessRequestDto>>> GetAllAsync();

    Task<ApiResult<List<AccessRequestDto>>> GetMyRequestsAsync();

    Task<ApiResult<AccessRequestDto>> GetByIdAsync(Guid id);

    Task<ApiResult<AccessRequestDto>> CreateAsync(CreateAccessRequestRequest request);

    Task<ApiResult<AccessRequestDto>> UpdateAsync(Guid id, UpdateAccessRequestRequest request);

    Task<ApiResult> DeleteAsync(Guid id);
}

public sealed class AccessRequestApiClient(ApiHandler api) : ApiClientBase(api), IAccessRequestApiClient
{
    public Task<ApiResult<List<AccessRequestDto>>> GetAllAsync() =>
        GetAsync<List<AccessRequestDto>>(ApiEndpoints.AccessRequests);

    public Task<ApiResult<List<AccessRequestDto>>> GetMyRequestsAsync() =>
        GetAsync<List<AccessRequestDto>>(ApiEndpoints.MyAccessRequests);

    public Task<ApiResult<AccessRequestDto>> GetByIdAsync(Guid id) =>
        GetAsync<AccessRequestDto>(ApiEndpoints.AccessRequest(id));

    public Task<ApiResult<AccessRequestDto>> CreateAsync(CreateAccessRequestRequest request) =>
        PostAsync<AccessRequestDto>(ApiEndpoints.AccessRequests, request);

    public Task<ApiResult<AccessRequestDto>> UpdateAsync(Guid id, UpdateAccessRequestRequest request) =>
        PutAsync<AccessRequestDto>(ApiEndpoints.AccessRequest(id), request);

    public Task<ApiResult> DeleteAsync(Guid id) =>
        DeleteAsync(ApiEndpoints.AccessRequest(id));
}
