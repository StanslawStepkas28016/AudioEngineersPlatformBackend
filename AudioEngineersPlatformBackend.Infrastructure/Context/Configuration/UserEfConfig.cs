using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Context.Configuration;

public class UserEfConfig : IEntityTypeConfiguration<User>
{ 
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.IdUser)
            .HasName("PK_User");

        builder
            .Property(u => u.IdUser)
            .IsRequired();

        builder
            .Property(u => u.FirstName)
            .IsRequired();

        builder
            .Property(u => u.LastName)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .IsRequired();

        builder
            .Property(u => u.PhoneNumber)
            .IsRequired();

        builder
            .Property(u => u.Password)
            .IsRequired();

        builder
            .HasOne(u => u.Role)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.IdRole)
            .HasConstraintName("FK_User_Role")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(u => u.UserLog)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.IdUserLog)
            .HasConstraintName("FK_User_UserLog")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasIndex(u => u.IdUser)
            .HasDatabaseName("IX_User_ForCheckAuth")
            .IncludeProperties(u => new
            {
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.IdRole
            });

        builder
            .ToTable("User");
    }
}