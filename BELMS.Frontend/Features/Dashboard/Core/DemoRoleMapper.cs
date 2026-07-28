namespace BELMS.Frontend.Features.Dashboard.Core;

public static class DemoRoleMapper
{
    public static string ResolveRoleFromEmail(string email)
    {
        var localPart = email.Split('@')[0].ToLowerInvariant();
        return localPart switch
        {
            "admin" => BelmsRoles.Admin,
            "hr" => BelmsRoles.Hr,
            "manager" => BelmsRoles.Manager,
            "it" => BelmsRoles.It,
            "security" => BelmsRoles.Security,
            "employee" => BelmsRoles.Employee,
            _ => BelmsRoles.Employee
        };
    }
}
