using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Context;

public class EngineersPlatformDbContext : DbContext
{
    public EngineersPlatformDbContext(DbContextOptions<EngineersPlatformDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserLog> UserLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EngineersPlatformDbContext).Assembly);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            // Seed Role Entity
            var adminRole = new Role(new Guid("5C9A3C43-8F4E-4C1E-A5F3-8E3CDBE0158A"), "Admin");
            var clientRole = new Role(new Guid("AAAA3C43-8F4E-4C1E-A5F3-8E3CDBE0158A"), "Client");
            var audioEngineerRole = new Role(new Guid("BBBB3C43-8F4E-4C1E-A5F3-8E3CDBE0158A"), "Audio engineer");
            modelBuilder.Entity<Role>().HasData(
                adminRole, clientRole, audioEngineerRole
            );

            // Seed User Entity
        }
    }
}