using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class AuditLogRepository(AppDbContext context) : IAuditLogRepository
{
    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await context.AuditLogs
            .Include(x => x.PerformedByUser)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await context.AuditLogs.AddAsync(auditLog);
    }

    public async Task<int> CountAsync()
    {
        return await context.AuditLogs.CountAsync(x => !x.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
