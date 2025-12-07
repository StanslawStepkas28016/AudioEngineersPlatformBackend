using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class AdvertEfConfig : IEntityTypeConfiguration<Advert>
{
    public void Configure(
        EntityTypeBuilder<Advert> builder
    )
    {
        builder
            .HasKey(a => a.IdAdvert)
            .HasName("PK_Advert");

        builder
            .Property(a => a.IdAdvert)
            .IsRequired();

        builder
            .Property(a => a.Title)
            .IsRequired();

        builder
            .Property(a => a.Description)
            .IsRequired();

        builder
            .HasIndex(a => new { a.Title, a.Description })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");

        builder
            .Property(a => a.CoverImageKey)
            .IsRequired();

        builder
            .Property(a => a.PortfolioUrl)
            .IsRequired();

        builder
            .Property(a => a.Price)
            .IsRequired();

        builder
            .HasOne(a => a.AdvertCategory)
            .WithMany(ac => ac.Adverts)
            .HasForeignKey(a => a.IdAdvertCategory)
            .HasConstraintName("FK_Advert_AdvertCategory")
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(a => a.AdvertLog)
            .WithMany(al => al.Adverts)
            .HasForeignKey(a => a.IdAdvertLog)
            .HasConstraintName("FK_Advert_AdvertLog")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(a => a.User)
            .WithMany(u => u.Adverts)
            .HasForeignKey(a => a.IdUser)
            .HasConstraintName("FK_Advert_User")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("Advert");
    }
}