using BELMS.Frontend.Features.Dashboard.Core;

namespace BELMS.Frontend.Theme;

public static class RoleAccents
{
    public static string GetAccent(string role) => role switch
    {
        BelmsRoles.Employee => "#2196F3",
        BelmsRoles.Hr => "#9C27B0",
        BelmsRoles.Manager => "#009688",
        BelmsRoles.It => "#2196F3",
        BelmsRoles.Security => "#800000",
        BelmsRoles.Admin => "#FF9800",
        _ => "#2196F3"
    };

    public static string GetCssClass(string role) => role switch
    {
        BelmsRoles.Employee => "role-employee",
        BelmsRoles.Hr => "role-hr",
        BelmsRoles.Manager => "role-manager",
        BelmsRoles.It => "role-it",
        BelmsRoles.Security => "role-security",
        BelmsRoles.Admin => "role-admin",
        _ => "role-employee"
    };
}
