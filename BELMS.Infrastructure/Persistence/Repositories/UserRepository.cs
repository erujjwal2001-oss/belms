using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<User>> GetByRoleAsync(Role role)
    {
        return await context.Users
            .Where(x => x.Role == role && !x.IsDeleted)
            .ToListAsync();
    }
}
