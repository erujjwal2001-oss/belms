using BELMS.Application.Interfaces.IService;

namespace BELMS.Application.Services.Dashboard;

/// <summary>
/// Factory that selects the registered <see cref="IDashboardViewBuilder"/> matching a role.
/// Falls back to the Employee builder for unknown roles or plain employees.
/// </summary>
public sealed class DashboardBuilderFactory(IEnumerable<IDashboardViewBuilder> builders) : IDashboardBuilderFactory
{
    private const string DefaultRole = "Employee";

    private readonly Dictionary<string, IDashboardViewBuilder> _builders =
        builders.ToDictionary(b => b.Role, StringComparer.OrdinalIgnoreCase);

    public IDashboardViewBuilder Create(string? role)
    {
        if (!string.IsNullOrWhiteSpace(role) && _builders.TryGetValue(role, out var builder))
        {
            return builder;
        }

        return _builders[DefaultRole];
    }
}
