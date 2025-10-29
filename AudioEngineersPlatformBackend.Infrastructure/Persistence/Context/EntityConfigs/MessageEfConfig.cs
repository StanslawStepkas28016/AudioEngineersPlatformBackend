using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class MessageEfConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder
            .HasKey(m => m.IdMessage)
            .HasName("PK_Message");

        builder
            .Property(m => m.TextContent)
            .IsRequired(false);

        builder
            .Property(m => m.FileName)
            .IsRequired(false);

        builder
            .Property(m => m.FileKey)
            .IsRequired();

        builder
            .Property(m => m.DateSent)
            .IsRequired();

        builder
            .ToTable("Message");
    }
}