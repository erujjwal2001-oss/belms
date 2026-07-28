using BELMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BELMS.Infrastructure.Persistence.Configurations;

public class WorkflowAttachmentConfiguration : IEntityTypeConfiguration<WorkflowAttachment>
{
    public void Configure(EntityTypeBuilder<WorkflowAttachment> builder)
    {
        builder.ToTable("WorkflowAttachments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.FilePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.WorkflowTask)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.WorkflowTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UploadedByUser)
            .WithMany()
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
