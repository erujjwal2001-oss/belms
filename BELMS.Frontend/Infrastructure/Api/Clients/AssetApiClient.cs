using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface IAssetApiClient
{
    Task<ApiResult<List<AssetDto>>> GetAllAsync();

    Task<ApiResult<AssetDto>> GetByIdAsync(Guid id);

    Task<ApiResult<AssetDto>> CreateAsync(CreateAssetRequest request);

    Task<ApiResult<AssetDto>> UpdateAsync(Guid id, UpdateAssetRequest request);

    Task<ApiResult> DeleteAsync(Guid id);

    Task<ApiResult<byte[]>> ExportAsync();
}

public sealed class AssetApiClient(ApiHandler api) : ApiClientBase(api), IAssetApiClient
{
    public Task<ApiResult<List<AssetDto>>> GetAllAsync() =>
        GetAsync<List<AssetDto>>(ApiEndpoints.Assets);

    public Task<ApiResult<AssetDto>> GetByIdAsync(Guid id) =>
        GetAsync<AssetDto>(ApiEndpoints.Asset(id));

    public Task<ApiResult<AssetDto>> CreateAsync(CreateAssetRequest request) =>
        PostAsync<AssetDto>(ApiEndpoints.Assets, request);

    public Task<ApiResult<AssetDto>> UpdateAsync(Guid id, UpdateAssetRequest request) =>
        PutAsync<AssetDto>(ApiEndpoints.Asset(id), request);

    public Task<ApiResult> DeleteAsync(Guid id) =>
        DeleteAsync(ApiEndpoints.Asset(id));

    public Task<ApiResult<byte[]>> ExportAsync() =>
        DownloadAsync(ApiEndpoints.ExportAssets);
}
