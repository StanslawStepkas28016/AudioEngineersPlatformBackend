using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class AdvertCategoryEfConfig : IEntityTypeConfiguration<AdvertCategory>
{
    public void Configure(EntityTypeBuilder<AdvertCategory> builder)
    {
        builder
            .HasKey(ac => ac.IdAdvertCategory)
            .HasName("PK_AdvertCategory");

        builder
            .Property(ac => ac.IdAdvertCategory)
            .IsRequired();

        builder
            .Property(ac => ac.CategoryName)
            .IsRequired();

        builder
            .ToTable("AdvertCategory");
    }
}