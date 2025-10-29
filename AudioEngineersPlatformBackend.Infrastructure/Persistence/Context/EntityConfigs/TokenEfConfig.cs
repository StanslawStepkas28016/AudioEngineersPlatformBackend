using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class TokenEfConfig : IEntityTypeConfiguration<Token>
{
    public void Configure(
        EntityTypeBuilder<Token> builder
    )
    {
        builder
            .HasKey(ut => ut.IdToken);

        builder
            .Property(ut => ut.Name)
            .IsRequired();

        builder
            .Property(ut => ut.ExpirationDate)
            .IsRequired();

        builder
            .Property(ut => ut.IdUser)
            .IsRequired();

        builder
            .HasOne(u => u.User)
            .WithMany(ul => ul.Tokens)
            .HasForeignKey(u => u.IdUser)
            .HasConstraintName("FK_User_Token")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable(nameof(Token));
    }
}