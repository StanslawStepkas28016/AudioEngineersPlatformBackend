using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class ReviewEfConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder
            .HasKey(r => r.IdReview)
            .HasName("PK_Review");

        builder
            .Property(r => r.Content)
            .IsRequired();

        builder
            .Property(r => r.SatisfactionLevel)
            .IsRequired();

        builder
            .Property(r => r.IdUser)
            .IsRequired();

        builder
            .HasOne(r => r.ReviewLog)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.IdReview)
            .HasConstraintName("FK_Review_ReviewLog")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Advert)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.IdAdvert)
            .HasConstraintName("FK_Review_Advert")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.User)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.IdUser)
            .HasConstraintName("FK_Review_User")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("Review");
    }
}