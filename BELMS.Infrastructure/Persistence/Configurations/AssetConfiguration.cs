using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssetName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.SerialNumber)
            .IsUnique();

        builder.Property(x => x.AssetType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
    }
}
