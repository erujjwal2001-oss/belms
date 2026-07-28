using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IRepo;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid id);

    Task<List<Notification>> GetByUserIdAsync(Guid userId);

    Task AddAsync(Notification notification);

    Task UpdateAsync(Notification notification);

    Task SaveChangesAsync();
}
