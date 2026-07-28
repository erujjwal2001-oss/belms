using BELMS.Application.DTOs.Dashboard;
using BELMS.Application.Services.Dashboard;

namespace BELMS.Application.Interfaces.IService;

/// <summary>
/// Builds a role-specific dashboard view from a shared <see cref="DashboardSnapshot"/>.
/// Each implementation handles exactly one role (Strategy participant of the Factory).
/// </summary>
public interface IDashboardViewBuilder
{
    /// <summary>The BELMS role name this builder is responsible for (e.g. "HR", "Admin").</summary>
    string Role { get; }

    RoleDashboardDto Build(DashboardSnapshot snapshot);
}

/// <summary>
/// Resolves the correct <see cref="IDashboardViewBuilder"/> for a given role,
/// falling back to the Employee builder when a role has no dedicated builder.
/// </summary>
public interface IDashboardBuilderFactory
{
    IDashboardViewBuilder Create(string? role);
}
