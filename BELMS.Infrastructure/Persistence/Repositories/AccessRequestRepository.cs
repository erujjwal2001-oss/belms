using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Infrastructure.Persistence.Repositories;

public class AccessRequestRepository(AppDbContext context) : IAccessRequestRepository
{
    public async Task<AccessRequest?> GetByIdAsync(Guid id)
    {
        return await context.AccessRequests
            .Include(x => x.Employee)
            .Include(x => x.RequestedByUser)
            .Include(x => x.ApprovedByUser)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public IQueryable<AccessRequest> Query()
    {
        return context.AccessRequests
            .Include(x => x.Employee)
            .Include(x => x.RequestedByUser)
            .Where(x => !x.IsDeleted);
    }

    public async Task<int> CountPendingAsync()
    {
        return await context.AccessRequests
            .CountAsync(x => !x.IsDeleted && x.Status == DomainTaskStatus.Pending);
    }

    public async Task AddAsync(AccessRequest accessRequest)
    {
        await context.AccessRequests.AddAsync(accessRequest);
    }

    public Task UpdateAsync(AccessRequest accessRequest)
    {
        context.AccessRequests.Update(accessRequest);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AccessRequest accessRequest)
    {
        accessRequest.IsDeleted = true;
        context.AccessRequests.Update(accessRequest);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
