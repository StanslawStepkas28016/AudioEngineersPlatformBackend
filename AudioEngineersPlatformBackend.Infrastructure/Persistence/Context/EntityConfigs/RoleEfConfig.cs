using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class RoleEfConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .HasKey(r => r.IdRole)
            .HasName("PK_Role");

        builder
            .Property(r => r.IdRole)
            .IsRequired();

        builder
            .Property(r => r.RoleName)
            .IsRequired();
        
        builder
            .ToTable("Role");
    }
}