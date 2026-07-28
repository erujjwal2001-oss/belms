using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BELMS.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddBelmsAuthentication(configuration);

        return services;
    }
}
