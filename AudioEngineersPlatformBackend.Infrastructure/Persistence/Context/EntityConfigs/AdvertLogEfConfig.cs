using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class AdvertLogEfConfig : IEntityTypeConfiguration<AdvertLog>
{
    public void Configure(EntityTypeBuilder<AdvertLog> builder)
    {
        builder
            .HasKey(al => al.IdAdvertLog)
            .HasName("PK_AdvertLog");

        builder
            .Property(al => al.IdAdvertLog)
            .IsRequired();

        builder
            .Property(al => al.DateCreated)
            .IsRequired();

        builder
            .Property(al => al.DateModified)
            .IsRequired(false);

        builder
            .Property(al => al.DateDeleted)
            .IsRequired(false);

        builder
            .Property(al => al.IsDeleted)
            .IsRequired();

        builder
            .Property(al => al.IsActive)
            .IsRequired();

        builder
            .ToTable("AdvertLog");
    }
}