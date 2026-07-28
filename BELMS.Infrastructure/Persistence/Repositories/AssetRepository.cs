using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class AssetRepository(AppDbContext context) : IAssetRepository
{
    public async Task<Asset?> GetByIdAsync(Guid id)
    {
        return await context.Assets
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Asset?> GetBySerialNumberAsync(string serialNumber)
    {
        return await context.Assets
            .FirstOrDefaultAsync(x => x.SerialNumber == serialNumber && !x.IsDeleted);
    }

    public async Task<int> CountAsync()
    {
        return await context.Assets.CountAsync(x => !x.IsDeleted);
    }

    public async Task<int> CountAssignedAsync()
    {
        return await context.EmployeeAssets
            .CountAsync(x => !x.IsDeleted && !x.IsReturned);
    }

    public IQueryable<Asset> Query()
    {
        return context.Assets.Where(x => !x.IsDeleted);
    }

    public async Task AddAsync(Asset asset)
    {
        await context.Assets.AddAsync(asset);
    }

    public Task UpdateAsync(Asset asset)
    {
        context.Assets.Update(asset);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Asset asset)
    {
        asset.IsDeleted = true;
        context.Assets.Update(asset);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
