using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Context.Configuration;

public class UserAuthLogEfConfig : IEntityTypeConfiguration<UserAuthLog>
{
    public void Configure(EntityTypeBuilder<UserAuthLog> builder)
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
            .Property(ul => ul.DateDeleted)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsDeleted)
            .IsRequired();

        builder
            .Property(ul => ul.RefreshToken)
            .IsRequired(false);

        builder
            .Property(ul => ul.RefreshTokenExpiration)
            .IsRequired(false);

        builder
            .Property(ul => ul.DateLastLogin)
            .IsRequired(false);

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
            .Property(ul => ul.ResetEmailToken)
            .IsRequired(false);

        builder
            .Property(ul => ul.ResetEmailTokenExpiration)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsResettingEmail)
            .IsRequired();

        builder
            .Property(ul => ul.ResetPasswordToken)
            .IsRequired(false);

        builder
            .Property(ul => ul.ResetPasswordTokenExpiration)
            .IsRequired(false);

        builder
            .Property(ul => ul.IsResettingPassword)
            .IsRequired();

        builder
            .ToTable("UserAuthLog");
    }
}