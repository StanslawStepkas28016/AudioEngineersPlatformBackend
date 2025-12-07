using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class SocialMediaNameEfConfig : IEntityTypeConfiguration<SocialMediaName>
{
    public void Configure(EntityTypeBuilder<SocialMediaName> builder)
    {
        builder
            .HasKey(s => s.IdSocialMediaName)
            .HasName("PK_SocialMediaName");

        builder
            .Property(s => s.IdSocialMediaName)
            .IsRequired();

        builder
            .Property(s => s.Name)
            .IsRequired();

        builder
            .ToTable("SocialMediaName");
    }
}