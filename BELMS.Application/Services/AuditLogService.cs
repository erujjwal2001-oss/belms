using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Entities;

namespace BELMS.Application.Services;

public class AuditLogService(IAuditLogRepository auditLogRepository) : IAuditLogService
{
    public async Task LogAsync(
        string entityName,
        Guid entityId,
        string action,
        Guid? performedByUserId,
        string? oldValues = null,
        string? newValues = null)
    {
        // Append-only audit record; no update/delete
        await auditLogRepository.AddAsync(new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            PerformedByUserId = performedByUserId,
            OldValues = oldValues,
            NewValues = newValues
        });

        await auditLogRepository.SaveChangesAsync();
    }
}
