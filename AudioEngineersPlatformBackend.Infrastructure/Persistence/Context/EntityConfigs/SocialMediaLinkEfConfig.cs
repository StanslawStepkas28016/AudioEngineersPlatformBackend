using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class SocialMediaLinkEfConfig : IEntityTypeConfiguration<SocialMediaLink>
{
    public void Configure(EntityTypeBuilder<SocialMediaLink> builder)
    {
        builder
            .HasKey(sml => sml.IdSocialMediaLink)
            .HasName("PK_SocialMediaLink");

        builder
            .Property(sml => sml.IdSocialMediaLink)
            .IsRequired();

        builder
            .Property(sml => sml.Url)
            .IsRequired();

        builder
            .HasOne(sml => sml.User)
            .WithMany(u => u.SocialMediaLinks)
            .HasForeignKey(sml => sml.IdUser)
            .HasConstraintName("FK_SocialMediaLink_User")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(sml => sml.SocialMediaName)
            .WithMany(smn => smn.SocialMediaLinks)
            .HasForeignKey(sml => sml.IdSocialMediaName)
            .HasConstraintName("FK_SocialMediaLink_SocialMediaName")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("SocialMediaLink");
    }
}