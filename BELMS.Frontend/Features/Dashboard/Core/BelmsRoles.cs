namespace BELMS.Frontend.Features.Dashboard.Core;

public static class BelmsRoles
{
    public const string Employee = "Employee";
    public const string Hr = "HR";
    public const string Manager = "Manager";
    public const string It = "IT";
    public const string Security = "Security";
    public const string Admin = "Admin";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Employee, Hr, Manager, It, Security, Admin
    };
}
