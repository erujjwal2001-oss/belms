using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.ToTable("WorkflowDefinitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasMany(x => x.Steps)
            .WithOne(x => x.WorkflowDefinition)
            .HasForeignKey(x => x.WorkflowDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
