namespace BELMS.Application.Interfaces.IService;

public interface IAuditLogService
{
    Task LogAsync(
        string entityName,
        Guid entityId,
        string action,
        Guid? performedByUserId,
        string? oldValues = null,
        string? newValues = null);
}
