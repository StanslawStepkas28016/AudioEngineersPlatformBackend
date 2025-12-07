using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class UserAuthLogEfConfig : IEntityTypeConfiguration<UserAuthLog>
{
    public void Configure(
        EntityTypeBuilder<UserAuthLog> builder
    )
    {
        builder
            .HasKey(ul => ul.IdUserAuthLog)
            .HasName("PK_UserAuthLog");

        builder
            .Property(ul => ul.IdUserAuthLog)
            .IsRequired();

        builder
            .Property(ul => ul.DateCreated)
            .IsRequired();

        builder
            .Property(ul => ul.DateLastLogin)
            .IsRequired(false);

        builder
            .Property(ul => ul.DateDeleted)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsDeleted)
            .IsRequired();

        builder
            .Property(ul => ul.IsVerified)
            .IsRequired();

        builder
            .Property(ul => ul.IsResettingEmail)
            .IsRequired();

        builder
            .Property(ul => ul.IsResettingPassword)
            .IsRequired();

        builder
            .Property(ul => ul.IsRemindingPassword)
            .IsRequired();
        
        builder
            .ToTable("UserAuthLog");
    }
}