using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    private const string OnboardingWorkflowName = "Employee Onboarding Workflow";

    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        await SeedUsersAsync(context, passwordHasher);
        await SeedWorkflowDefinitionsAsync(context);
    }

    private static async Task SeedUsersAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var users = new List<User>
        {
            CreateUser("System Admin", "admin@laxmisunrise.com", "Admin@123", Role.Admin, passwordHasher),
            CreateUser("HR Manager", "hr@laxmisunrise.com", "Hr@12345", Role.HR, passwordHasher),
            CreateUser("Department Manager", "manager@laxmisunrise.com", "Manager@123", Role.Manager, passwordHasher),
            CreateUser("IT Support", "it@laxmisunrise.com", "It@12345", Role.IT, passwordHasher),
            CreateUser("Security Officer", "security@laxmisunrise.com", "Security@123", Role.Security, passwordHasher)
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    private static async Task SeedWorkflowDefinitionsAsync(AppDbContext context)
    {
        if (await context.WorkflowDefinitions.AnyAsync(x => x.Name == OnboardingWorkflowName))
        {
            return;
        }

        var workflow = new WorkflowDefinition
        {
            Name = OnboardingWorkflowName,
            Description = "Standard employee onboarding: HR → Manager → IT → Security",
            IsActive = true,
            Steps =
            [
                new WorkflowStepDefinition
                {
                    StepOrder = 1,
                    Name = "HR approval",
                    AssignedRole = Role.HR,
                    StepType = "Approval"
                },
                new WorkflowStepDefinition
                {
                    StepOrder = 2,
                    Name = "Manager approval",
                    AssignedRole = Role.Manager,
                    StepType = "Approval"
                },
                new WorkflowStepDefinition
                {
                    StepOrder = 3,
                    Name = "IT asset assignment",
                    AssignedRole = Role.IT,
                    StepType = "AssetAssignment"
                },
                new WorkflowStepDefinition
                {
                    StepOrder = 4,
                    Name = "Security clearance",
                    AssignedRole = Role.Security,
                    StepType = "Clearance"
                }
            ]
        };

        await context.WorkflowDefinitions.AddAsync(workflow);
        await context.SaveChangesAsync();
    }

    private static User CreateUser(
        string fullName,
        string email,
        string password,
        Role role,
        IPasswordHasher passwordHasher)
    {
        return new User
        {
            FullName = fullName,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(password),
            Role = role
        };
    }
}
