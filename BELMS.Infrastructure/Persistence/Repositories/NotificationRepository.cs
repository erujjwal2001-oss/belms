using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<Notification>> GetByUserIdAsync(Guid userId)
    {
        return await context.Notifications
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Notification notification)
    {
        await context.Notifications.AddAsync(notification);
    }

    public Task UpdateAsync(Notification notification)
    {
        context.Notifications.Update(notification);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
