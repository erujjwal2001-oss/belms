using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface IDashboardApiClient
{
    Task<ApiResult<RoleDashboardDto>> GetOverviewAsync();

    Task<ApiResult<byte[]>> ExportAuditLogsAsync();
}

public sealed class DashboardApiClient(ApiHandler api) : ApiClientBase(api), IDashboardApiClient
{
    public Task<ApiResult<RoleDashboardDto>> GetOverviewAsync() =>
        GetAsync<RoleDashboardDto>(ApiEndpoints.DashboardOverview);

    public Task<ApiResult<byte[]>> ExportAuditLogsAsync() =>
        DownloadAsync(ApiEndpoints.ExportAuditLogs);
}
