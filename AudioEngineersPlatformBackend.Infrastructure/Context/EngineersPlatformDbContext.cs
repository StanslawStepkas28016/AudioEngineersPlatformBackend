using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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

            // Seed UserLog with User Entities (mock users)
            var passwordHasher = new PasswordHasher<User>();

            var ul1 = UserLog.Create();
            ul1.VerifyUserAccount();
            var u1 = new User("Dominik", "Kowalski", "dominik.kow@gmail.com", "+48123456789", "test",
                adminRole.IdRole, ul1.IdUserLog);
            u1.SetHashedPassword(passwordHasher.HashPassword(u1, u1.Password));

            var ul2 = UserLog.Create();
            ul2.VerifyUserAccount();
            var u2 = new User("Jan", "Nowak", "jan.nowak@gmail.com", "+48696432123", "test", clientRole.IdRole,
                ul2.IdUserLog);
            u2.SetHashedPassword(passwordHasher.HashPassword(u2, u2.Password));


            var ul3 = UserLog.Create();
            ul3.VerifyUserAccount();
            var u3 = new User("Anna", "Kowalska", "anna.kow@gmail.com", "+48543123123", "test",
                audioEngineerRole.IdRole, ul3.IdUserLog);
            u3.SetHashedPassword(passwordHasher.HashPassword(u3, u3.Password));

            modelBuilder.Entity<UserLog>().HasData(
                ul1, ul2, ul3
            );
            
            modelBuilder.Entity<User>().HasData(
                u1, u2, u3
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