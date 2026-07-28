using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IRepo;

public interface IAssetRepository
{
    Task<Asset?> GetByIdAsync(Guid id);

    Task<Asset?> GetBySerialNumberAsync(string serialNumber);

    Task<int> CountAsync();

    Task<int> CountAssignedAsync();

    IQueryable<Asset> Query();

    Task AddAsync(Asset asset);

    Task UpdateAsync(Asset asset);

    Task DeleteAsync(Asset asset);

    Task SaveChangesAsync();
}
