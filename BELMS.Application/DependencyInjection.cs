using AutoMapper;
using BELMS.Application.DTOs.Employees;
using BELMS.Application.Features.Employees.Validators;
using BELMS.Application.Interfaces.IService;
using BELMS.Application.Mappings;
using BELMS.Application.Services;
using BELMS.Application.Services.Dashboard;
using BELMS.Application.Services.Dashboard.Builders;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BELMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddScoped<IValidator<CreateEmployeeRequest>, EmployeeCreateEmployeeRequestValidator>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWorkflowDefinitionService, WorkflowDefinitionService>();
        services.AddScoped<IWorkflowInstanceService, WorkflowInstanceService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IAccessRequestService, AccessRequestService>();
        services.AddScoped<IWorkflowExecutionService, WorkflowExecutionService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Rich role-aware dashboard (Factory pattern over per-role view builders)
        services.AddScoped<IDashboardViewBuilder, EmployeeDashboardBuilder>();
        services.AddScoped<IDashboardViewBuilder, HrDashboardBuilder>();
        services.AddScoped<IDashboardViewBuilder, ManagerDashboardBuilder>();
        services.AddScoped<IDashboardViewBuilder, ItDashboardBuilder>();
        services.AddScoped<IDashboardViewBuilder, SecurityDashboardBuilder>();
        services.AddScoped<IDashboardViewBuilder, AdminDashboardBuilder>();
        services.AddScoped<IDashboardBuilderFactory, DashboardBuilderFactory>();
        services.AddScoped<IRoleDashboardService, RoleDashboardService>();

        return services;
    }
}
