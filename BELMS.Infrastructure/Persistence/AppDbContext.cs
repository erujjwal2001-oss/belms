using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowStepDefinition> WorkflowStepDefinitions => Set<WorkflowStepDefinition>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<EmployeeAsset> EmployeeAssets => Set<EmployeeAsset>();
    public DbSet<WorkflowInstance> WorkflowInstances => Set<WorkflowInstance>();
    public DbSet<WorkflowTask> WorkflowTasks => Set<WorkflowTask>();
    public DbSet<AccessRequest> AccessRequests => Set<AccessRequest>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<WorkflowAttachment> WorkflowAttachments => Set<WorkflowAttachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
