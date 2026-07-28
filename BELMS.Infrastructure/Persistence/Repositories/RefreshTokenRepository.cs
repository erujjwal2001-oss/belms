using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public Task UpdateAsync(RefreshToken refreshToken)
    {
        context.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
