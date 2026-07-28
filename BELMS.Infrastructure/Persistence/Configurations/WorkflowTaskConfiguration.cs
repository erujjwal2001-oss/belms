using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class WorkflowTaskConfiguration : IEntityTypeConfiguration<WorkflowTask>
{
    public void Configure(EntityTypeBuilder<WorkflowTask> builder)
    {
        builder.ToTable("WorkflowTasks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StepName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.StepType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.AssignedRole)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Comments)
            .HasMaxLength(1000);

        builder.HasOne(x => x.CompletedByUser)
            .WithMany()
            .HasForeignKey(x => x.CompletedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(x => new { x.WorkflowInstanceId, x.StepOrder })
            .IsUnique();
    }
}
