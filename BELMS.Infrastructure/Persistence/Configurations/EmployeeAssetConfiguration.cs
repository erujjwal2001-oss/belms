using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class EmployeeAssetConfiguration : IEntityTypeConfiguration<EmployeeAsset>
{
    public void Configure(EntityTypeBuilder<EmployeeAsset> builder)
    {
        builder.ToTable("EmployeeAssets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignedAt)
            .IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeAssets)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Asset)
            .WithMany(x => x.EmployeeAssets)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.EmployeeId, x.AssetId, x.IsReturned });
    }
}
