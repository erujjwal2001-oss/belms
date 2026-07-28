using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Infrastructure.Persistence;
using BELMS.Infrastructure.Persistence.Repositories;
using BELMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BELMS.Infrastructure.Extensions;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
        services.AddScoped<IWorkflowDefinitionRepository, WorkflowDefinitionRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
        services.AddScoped<IExcelService, Services.ExcelService>();

        return services;
    }
}
