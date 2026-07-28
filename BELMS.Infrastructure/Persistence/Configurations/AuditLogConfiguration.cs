using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.OldValues)
            .HasMaxLength(4000);

        builder.Property(x => x.NewValues)
            .HasMaxLength(4000);

        builder.HasOne(x => x.PerformedByUser)
            .WithMany()
            .HasForeignKey(x => x.PerformedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.EntityName, x.EntityId });
        builder.HasIndex(x => x.CreatedAt);
    }
}
