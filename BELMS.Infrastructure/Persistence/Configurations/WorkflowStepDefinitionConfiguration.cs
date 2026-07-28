using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class WorkflowStepDefinitionConfiguration : IEntityTypeConfiguration<WorkflowStepDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowStepDefinition> builder)
    {
        builder.ToTable("WorkflowStepDefinitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.StepType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.AssignedRole)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => new { x.WorkflowDefinitionId, x.StepOrder })
            .IsUnique();
    }
}
