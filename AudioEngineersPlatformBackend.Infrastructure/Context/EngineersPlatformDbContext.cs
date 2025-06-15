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
    public virtual DbSet<Advert> Adverts { get; set; }
    public virtual DbSet<AdvertCategory> AdvertCategories { get; set; }
    public virtual DbSet<AdvertLog> AdvertLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EngineersPlatformDbContext).Assembly);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            // Seed Role Entity
            var adminRole = new Role("Administrator");
            var clientRole = new Role("Client");
            var audioEngineerRole = new Role("Audio engineer");

            modelBuilder.Entity<Role>().HasData(
                adminRole, clientRole, audioEngineerRole
            );

            // Seed AdvertCategory Entity
            var mixingCategory = new AdvertCategory("Mixing");
            var masteringCategory = new AdvertCategory("Mastering");
            var productionCategory = new AdvertCategory("Production");

            modelBuilder.Entity<AdvertCategory>().HasData(
                mixingCategory, masteringCategory, productionCategory
            );
        }
    }
}