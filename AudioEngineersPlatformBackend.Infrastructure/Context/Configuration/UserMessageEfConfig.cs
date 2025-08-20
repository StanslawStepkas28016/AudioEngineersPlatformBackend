using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Context.Configuration;

public class UserMessageEfConfig : IEntityTypeConfiguration<UserMessage>
{
    public void Configure(EntityTypeBuilder<UserMessage> builder)
    {
        builder
            .HasKey(um => um.IdUserMessage)
            .HasName("PK_UserMessage");

        builder
            .HasOne(um => um.UserSender)
            .WithMany(um => um.UserMessagesSender)
            .HasForeignKey(um => um.IdUserSender)
            .HasConstraintName("FK_UserMessage_UserSender")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(um => um.UserRecipient)
            .WithMany(um => um.UserMessagesRecipient)
            .HasForeignKey(um => um.IdUserRecipient)
            .HasConstraintName("FK_UserMessage_UserRecipient")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(um => um.Message)
            .WithMany(um => um.UserMessages)
            .HasForeignKey(um => um.IdMessage)
            .HasConstraintName("FK_UserMessage_Message")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("UserMessage");
    }
}