using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context.EntityConfigs;

public class HubConnectionEfConfig : IEntityTypeConfiguration<HubConnection>
{
    public void Configure(EntityTypeBuilder<HubConnection> builder)
    {
        builder
            .HasKey(hc => hc.IdHubConnection)
            .HasName("PK_Connection");

        builder
            .Property(hc => hc.IdUser)
            .IsRequired();

        builder
            .Property(hc => hc.ConnectionId)
            .IsRequired();

        builder
            .HasOne(hc => hc.User)
            .WithMany(u => u.HubConnections)
            .HasForeignKey(hc => hc.IdUser)
            .HasConstraintName("FK_HubConnection_User")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("HubConnection");
    }
}