using AutoMapper;
using BELMS.Application.DTOs.Assets;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;

namespace BELMS.Application.Services;

public class AssetService(IAssetRepository assetRepository, IMapper mapper) : IAssetService
{
    public async Task<Result<AssetDto>> CreateAsync(CreateAssetRequest request)
    {
        var serial = request.SerialNumber.Trim();
        if (await assetRepository.GetBySerialNumberAsync(serial) is not null)
        {
            return Result<AssetDto>.Failure(
                Error.Conflict("Asset.SerialExists", AssetMessages.SerialNumberExists));
        }

        if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
        {
            return Result<AssetDto>.Failure(
                Error.Validation("Asset.InvalidType", ValidationMessages.ValidationFailed));
        }

        var asset = new Asset
        {
            AssetName = request.AssetName.Trim(),
            SerialNumber = serial,
            AssetType = assetType,
            IsAvailable = request.IsAvailable
        };

        await assetRepository.AddAsync(asset);
        await assetRepository.SaveChangesAsync();

        return Result<AssetDto>.Success(mapper.Map<AssetDto>(asset));
    }

    public async Task<Result<AssetDto>> GetByIdAsync(Guid id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset is null)
        {
            return Result<AssetDto>.Failure(Error.NotFound("Asset.NotFound", AssetMessages.NotFound));
        }

        return Result<AssetDto>.Success(mapper.Map<AssetDto>(asset));
    }

    public async Task<Result<List<AssetDto>>> GetAllAsync()
    {
        var assets = assetRepository.Query().OrderBy(x => x.AssetName).ToList();
        return Result<List<AssetDto>>.Success(mapper.Map<List<AssetDto>>(assets));
    }

    public async Task<Result<AssetDto>> UpdateAsync(Guid id, UpdateAssetRequest request)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset is null)
        {
            return Result<AssetDto>.Failure(Error.NotFound("Asset.NotFound", AssetMessages.NotFound));
        }

        if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
        {
            return Result<AssetDto>.Failure(
                Error.Validation("Asset.InvalidType", ValidationMessages.ValidationFailed));
        }

        asset.AssetName = request.AssetName.Trim();
        asset.AssetType = assetType;
        asset.IsAvailable = request.IsAvailable;

        await assetRepository.UpdateAsync(asset);
        await assetRepository.SaveChangesAsync();

        return Result<AssetDto>.Success(mapper.Map<AssetDto>(asset));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset is null)
        {
            return Result.Failure(Error.NotFound("Asset.NotFound", AssetMessages.NotFound));
        }

        await assetRepository.DeleteAsync(asset);
        await assetRepository.SaveChangesAsync();
        return Result.Success();
    }
}
