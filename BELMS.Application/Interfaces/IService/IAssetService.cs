using BELMS.Application.DTOs.Assets;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IAssetService
{
    Task<Result<AssetDto>> CreateAsync(CreateAssetRequest request);
    Task<Result<AssetDto>> GetByIdAsync(Guid id);
    Task<Result<List<AssetDto>>> GetAllAsync();
    Task<Result<AssetDto>> UpdateAsync(Guid id, UpdateAssetRequest request);
    Task<Result> DeleteAsync(Guid id);
}
