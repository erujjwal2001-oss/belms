namespace BELMS.Frontend.Infrastructure.Api;

/// <summary>
/// Relative paths for BELMS.Api endpoints consumed by the frontend.
/// </summary>
public static class ApiEndpoints
{
    // Auth
    public const string Login = "api/auth/login";
    public const string Refresh = "api/auth/refresh";
    public const string Logout = "api/auth/logout";

    // Dashboard
    public const string Dashboard = "api/dashboard";
    public const string DashboardOverview = "api/dashboard/overview";

    // Employees
    public const string Employees = "api/employees";
    public static string Employee(Guid id) => $"api/employees/{id}";

    // Assets
    public const string Assets = "api/assets";
    public static string Asset(Guid id) => $"api/assets/{id}";

    // Access requests
    public const string AccessRequests = "api/access-requests";
    public const string MyAccessRequests = "api/access-requests/my";
    public static string AccessRequest(Guid id) => $"api/access-requests/{id}";

    // Notifications
    public const string Notifications = "api/notifications";
    public const string MarkAllNotificationsRead = "api/notifications/read-all";
    public static string Notification(Guid id) => $"api/notifications/{id}";
    public static string MarkNotificationRead(Guid id) => $"api/notifications/{id}/read";

    // Workflow definitions
    public const string WorkflowDefinitions = "api/workflow-definitions";
    public static string WorkflowDefinition(Guid id) => $"api/workflow-definitions/{id}";

    // Workflow execution
    public const string PendingApprovals = "api/workflows/instances/pending-approvals";
    public static string WorkflowInstance(Guid id) => $"api/workflows/instances/{id}";
    public static string ApproveTask(Guid id) => $"api/workflows/instances/{id}/approve";
    public static string RejectTask(Guid id) => $"api/workflows/instances/{id}/reject";
    public static string ReturnTask(Guid id) => $"api/workflows/instances/{id}/return";
    public static string ResubmitTask(Guid id) => $"api/workflows/instances/{id}/resubmit";

    // Excel
    public const string ExportEmployees = "api/excel/employees/export";
    public const string ImportEmployees = "api/excel/employees/import";
    public const string ExportAssets = "api/excel/assets/export";
    public const string ExportWorkflows = "api/excel/workflows/export";
    public const string ExportAuditLogs = "api/excel/audit-logs/export";
}
