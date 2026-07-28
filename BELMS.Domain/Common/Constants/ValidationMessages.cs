namespace BELMS.Domain.Common.Constants;

public static class ValidationMessages
{
    public const string ValidationFailed = "One or more validation errors occurred.";
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Email format is invalid.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordWeak = "Password must contain at least one uppercase letter and one digit.";
    public const string RefreshTokenRequired = "Refresh token is required.";
    public const string RefreshTokenInvalid = "Refresh token is invalid.";
    public const string WorkflowNameRequired = "Workflow name is required.";
    public const string WorkflowStepsRequired = "At least one workflow step is required.";
    public const string EmployeeCodeRequired = "Employee code is required.";
    public const string EmployeeCodeAlreadyExists = "Employee code already exists.";
    public const string EmployeeEmailAlreadyExists = "Employee email already exists.";
    public const string EmployeeEmailDomainInvalid = "Email must belong to @laxmisunrise.com domain.";
    public const string FullNameRequired = "Full name is required.";
    public const string DepartmentRequired = "Department is required.";
    public const string DesignationRequired = "Designation is required.";
    public const string StepNameRequired = "Step name is required.";
    public const string StepTypeRequired = "Step type is required.";
}
