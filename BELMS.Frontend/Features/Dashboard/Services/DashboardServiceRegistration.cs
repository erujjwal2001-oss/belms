using BELMS.Frontend.Features.Dashboard.Admin.Services;
using BELMS.Frontend.Features.Dashboard.Employee.Services;
using BELMS.Frontend.Features.Dashboard.Hr.Services;
using BELMS.Frontend.Features.Dashboard.It.Services;
using BELMS.Frontend.Features.Dashboard.Manager.Services;
using BELMS.Frontend.Features.Dashboard.Security.Services;

namespace BELMS.Frontend.Features.Dashboard.Services;

public static class DashboardServiceRegistration
{
    /// <summary>
    /// Registers the role dashboard services. Each interface resolves to an API-backed
    /// implementation wrapped in a caching + logging Decorator.
    /// </summary>
    public static IServiceCollection AddBelmsDashboardServices(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddScoped<IRoleContext, RoleContext>();

        RegisterDecorated<IEmployeeDashboardService, ApiEmployeeDashboardService, EmployeeDashboardServiceDecorator>(services);
        RegisterDecorated<IHrDashboardService, ApiHrDashboardService, HrDashboardServiceDecorator>(services);
        RegisterDecorated<IManagerDashboardService, ApiManagerDashboardService, ManagerDashboardServiceDecorator>(services);
        RegisterDecorated<IItDashboardService, ApiItDashboardService, ItDashboardServiceDecorator>(services);
        RegisterDecorated<ISecurityDashboardService, ApiSecurityDashboardService, SecurityDashboardServiceDecorator>(services);
        RegisterDecorated<IAdminDashboardService, ApiAdminDashboardService, AdminDashboardServiceDecorator>(services);

        return services;
    }

    private static void RegisterDecorated<TInterface, TImplementation, TDecorator>(IServiceCollection services)
        where TInterface : class
        where TImplementation : class
        where TDecorator : class, TInterface
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<TInterface>(sp => ActivatorUtilities.CreateInstance<TDecorator>(
            sp,
            sp.GetRequiredService<TImplementation>()));
    }
}
