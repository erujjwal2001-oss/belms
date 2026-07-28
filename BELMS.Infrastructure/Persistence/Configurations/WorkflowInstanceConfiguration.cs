using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
    {
        builder.ToTable("WorkflowInstances");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkflowName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.EntityType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.WorkflowDefinition)
            .WithMany()
            .HasForeignKey(x => x.WorkflowDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.WorkflowInstance)
            .HasForeignKey(x => x.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
