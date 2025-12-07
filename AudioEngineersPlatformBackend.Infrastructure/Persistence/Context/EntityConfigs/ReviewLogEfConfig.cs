using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class ReviewLogEfConfig : IEntityTypeConfiguration<ReviewLog>
{
    public void Configure(EntityTypeBuilder<ReviewLog> builder)
    {
        builder
            .HasKey(rl => rl.IdReviewLog)
            .HasName("PK_ReviewLog");

        builder
            .Property(rl => rl.IdReviewLog)
            .IsRequired();

        builder
            .Property(rl => rl.DateCreated)
            .IsRequired();

        builder
            .Property(rl => rl.DateDeleted)
            .IsRequired(false);

        builder
            .Property(rl => rl.IsDeleted)
            .IsRequired();

        builder
            .HasMany(rl => rl.Reviews)
            .WithOne(r => r.ReviewLog)
            .HasForeignKey(r => r.IdReviewLog)
            .HasConstraintName("FK_Review_ReviewLog")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("ReviewLog");
    }
}