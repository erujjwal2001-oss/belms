using BELMS.Domain.Entities;
using BELMS.Domain.Enums;

namespace BELMS.Application.Interfaces.IRepo;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(Guid id);

    Task<List<User>> GetByRoleAsync(Role role);
}
