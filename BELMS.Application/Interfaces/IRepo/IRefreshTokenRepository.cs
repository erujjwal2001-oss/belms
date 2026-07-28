using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IRepo;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task UpdateAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
}
