namespace BELMS.Frontend.Features.Dashboard.Services;

public interface IRoleContext
{
    Task<string> GetRoleAsync(CancellationToken cancellationToken = default);

    Task<string> GetDisplayNameAsync(CancellationToken cancellationToken = default);

    Task<string> GetEmailAsync(CancellationToken cancellationToken = default);
}
