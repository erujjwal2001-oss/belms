using BELMS.Application.DTOs.Employees;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Common.Constants;
using FluentValidation;

namespace BELMS.Application.Features.Employees.Validators;

public class EmployeeCreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    private const string AllowedEmailDomain = "laxmisunrise.com";

    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeCreateEmployeeRequestValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(x => x.Email)
            .Must(BeAllowedEmailDomain).WithMessage(ValidationMessages.EmployeeEmailDomainInvalid)
            .MustAsync(BeUniqueEmail).WithMessage(ValidationMessages.EmployeeEmailAlreadyExists);
    }

    private static bool BeAllowedEmailDomain(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return true;
        }

        var normalized = email.Trim().ToLowerInvariant();
        return normalized.EndsWith($"@{AllowedEmailDomain}");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return true;
        }

        var existing = await _employeeRepository.GetByEmailAsync(email.Trim().ToLowerInvariant());
        return existing is null;
    }
}
