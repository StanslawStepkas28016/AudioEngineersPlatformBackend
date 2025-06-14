using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Context.Configuration;

public class UserLogEfConfig : IEntityTypeConfiguration<UserLog>
{
    public void Configure(EntityTypeBuilder<UserLog> builder)
    {
        builder
            .HasKey(ul => ul.IdUserLog)
            .HasName("PK_UserLog");

        builder
            .Property(ul => ul.IdUserLog)
            .IsRequired();

        builder
            .Property(ul => ul.DateCreated)
            .IsRequired();

        builder
            .Property(ul => ul.DateDeleted)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsDeleted)
            .IsRequired();

        builder
            .Property(ul => ul.VerificationCode)
            .IsRequired(false);

        builder
            .Property(ul => ul.VerificationCodeExpiration)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsVerified)
            .IsRequired();

        builder
            .Property(ul => ul.DateLastLogin)
            .IsRequired(false);

        builder
            .Property(ul => ul.RefreshToken)
            .IsRequired(false);

        builder
            .Property(ul => ul.RefreshTokenExp)
            .IsRequired(false);

        builder
            .ToTable("UserLog");
    }
}