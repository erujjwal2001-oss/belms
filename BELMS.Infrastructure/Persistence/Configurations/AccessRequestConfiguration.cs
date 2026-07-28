using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class AccessRequestConfiguration : IEntityTypeConfiguration<AccessRequest>
{
    public void Configure(EntityTypeBuilder<AccessRequest> builder)
    {
        builder.ToTable("AccessRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RequestedByUser)
            .WithMany()
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedByUser)
            .WithMany()
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
