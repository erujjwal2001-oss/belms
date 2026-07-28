using BELMS.Frontend.Infrastructure.Api.Clients;
using BELMS.Frontend.Infrastructure.Files;

namespace BELMS.Frontend.Infrastructure.Api;

public static class ApiClientRegistration
{
    /// <summary>Registers the typed API clients that sit on top of <see cref="ApiHandler"/>.</summary>
    public static IServiceCollection AddBelmsApiClients(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeApiClient, EmployeeApiClient>();
        services.AddScoped<IAssetApiClient, AssetApiClient>();
        services.AddScoped<IAccessRequestApiClient, AccessRequestApiClient>();
        services.AddScoped<INotificationApiClient, NotificationApiClient>();
        services.AddScoped<IWorkflowApiClient, WorkflowApiClient>();
        services.AddScoped<IDashboardApiClient, DashboardApiClient>();

        services.AddScoped<IFileDownloadService, FileDownloadService>();

        return services;
    }
}
