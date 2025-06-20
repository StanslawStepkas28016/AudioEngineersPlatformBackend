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

        /*
         * Data seeding for the database. The guids used in the seeding process are
         * overriden by the ones generated via the domain constructors in order to prevent
         * the seeding process from re-adding the same rows of data.
         */
        /*if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            // Seed Role Entity
            var adminRole = Role.Create("Administrator");
            adminRole.SetIdRoleForSeeding(Guid.Parse("D92D29B8-F462-46DF-8EFB-DE6B9AA5109A"));

            var clientRole = Role.Create("Client");
            clientRole.SetIdRoleForSeeding(Guid.Parse("004865E2-177F-4C54-BB4C-69799F0BF315"));

            var audioEngineerRole = Role.Create("Audio engineer");
            audioEngineerRole.SetIdRoleForSeeding(Guid.Parse("522C6700-165E-4189-B234-9FB533266E07"));

            modelBuilder.Entity<Role>().HasData(
                adminRole, clientRole, audioEngineerRole
            );

            // Seed UserLog with User Entities (mock users)
            var passwordHasher = new PasswordHasher<User>();

            var ul1 = UserLog.Create();
            ul1.SetIdUserLogForSeeding(Guid.Parse("5CB8EFAA-2432-46D1-9984-B41A40BAB7B3"));
            ul1.VerifyUserAccount();
            var u1 = User.Create("Dominik", "Kowalski", "dominik.kow@gmail.com", "+48123456789", "test",
                adminRole.IdRole,
                ul1.IdUserLog);
            u1.SetIdUserForSeeding(Guid.Parse("AEBC2724-0EDF-4691-99E9-65CBD3AAB3BF"));
            u1.SetHashedPassword(passwordHasher.HashPassword(u1, u1.Password));

            var ul2 = UserLog.Create();
            ul2.SetIdUserLogForSeeding(Guid.Parse("2F765163-6728-48BC-9767-66687EFDF86E"));
            ul2.VerifyUserAccount();
            var u2 = User.Create("Jan", "Nowak", "jan.nowak@gmail.com", "+48696432123", "test", clientRole.IdRole,
                ul2.IdUserLog);
            u2.SetIdUserForSeeding(Guid.Parse("5BFC9C8D-4789-4065-99D9-81EC5B58C0F5"));
            u2.SetHashedPassword(passwordHasher.HashPassword(u2, u2.Password));

            var ul3 = UserLog.Create();
            ul3.VerifyUserAccount();
            var u3 = User.Create("Anna", "Kowalska", "anna.kow@gmail.com", "+48543123123", "test",
                audioEngineerRole.IdRole, ul3.IdUserLog);
            u3.SetHashedPassword(passwordHasher.HashPassword(u3, u3.Password));

            var ul4 = UserLog.Create();
            ul4.VerifyUserAccount();
            var u4 = User.Create("Piotr", "Nowak", "piotr.nowak@example.com", "+48111222333", "test",
                audioEngineerRole.IdRole, ul4.IdUserLog);
            u4.SetHashedPassword(passwordHasher.HashPassword(u4, u4.Password));

            var ul5 = UserLog.Create();
            ul5.VerifyUserAccount();
            var u5 = User.Create("Ewa", "Maj", "ewa.maj@example.com", "+48111333444", "test", audioEngineerRole.IdRole,
                ul5.IdUserLog);
            u5.SetHashedPassword(passwordHasher.HashPassword(u5, u5.Password));

            var ul6 = UserLog.Create();
            ul6.VerifyUserAccount();
            var u6 = User.Create("Tomasz", "Zieliński", "tomasz.zielinski@example.com", "+48111444555", "test",
                audioEngineerRole.IdRole, ul6.IdUserLog);
            u6.SetHashedPassword(passwordHasher.HashPassword(u6, u6.Password));

            var ul7 = UserLog.Create();
            ul7.VerifyUserAccount();
            var u7 = User.Create("Michał", "Wójcik", "michal.wojcik@example.com", "+48111555666", "test",
                audioEngineerRole.IdRole, ul7.IdUserLog);
            u7.SetHashedPassword(passwordHasher.HashPassword(u7, u7.Password));

            var ul8 = UserLog.Create();
            ul8.VerifyUserAccount();
            var u8 = User.Create("Katarzyna", "Wiśniewska", "katarzyna.wisniewska@example.com", "+48111666777", "test",
                audioEngineerRole.IdRole, ul8.IdUserLog);
            u8.SetHashedPassword(passwordHasher.HashPassword(u8, u8.Password));

            var ul9 = UserLog.Create();
            ul9.VerifyUserAccount();
            var u9 = User.Create("Krzysztof", "Lewandowski", "krzysztof.lewandowski@example.com", "+48111777888",
                "test",
                audioEngineerRole.IdRole, ul9.IdUserLog);
            u9.SetHashedPassword(passwordHasher.HashPassword(u9, u9.Password));

            var ul10 = UserLog.Create();
            ul10.VerifyUserAccount();
            var u10 = User.Create("Agnieszka", "Wróbel", "agnieszka.wrobel@example.com", "+48111888999", "test",
                audioEngineerRole.IdRole, ul10.IdUserLog);
            u10.SetHashedPassword(passwordHasher.HashPassword(u10, u10.Password));

            var ul11 = UserLog.Create();
            ul11.VerifyUserAccount();
            var u11 = User.Create("Paweł", "Kamiński", "pawel.kaminski@example.com", "+48111999000", "test",
                audioEngineerRole.IdRole, ul11.IdUserLog);
            u11.SetHashedPassword(passwordHasher.HashPassword(u11, u11.Password));

            modelBuilder.Entity<UserLog>().HasData(
                ul1, ul2, ul3,
                ul4, ul5, ul6, ul7, ul8, ul9, ul10, ul11
            );

            modelBuilder.Entity<User>().HasData(
                u1, u2, u3,
                u4, u5, u6, u7, u8, u9, u10, u11
            );

            // Seed AdvertCategory Entity
            var mixingCategory = AdvertCategory.Create("Mixing");
            var masteringCategory = AdvertCategory.Create("Mastering");
            var productionCategory = AdvertCategory.Create("Production");

            modelBuilder.Entity<AdvertCategory>().HasData(
                mixingCategory, masteringCategory, productionCategory
            );

            // Seed the AdvertLog and Advert Entities
            var al1 = AdvertLog.Create();
            var a1 = Advert.Create(
                "I will mix your song professionally!",
                "With over 10 years of hands-on experience in music mixing, I meticulously balance every element of your track—from drums and bass to vocals and effects—to ensure a polished, radio-ready sound. " +
                "I use industry-standard tools and reference mixes to match the tonal character and loudness of top-charting songs. Whether you need depth, clarity, or that modern “in-your-face” sheen, I’ll tailor my approach to your genre and artistic vision. " +
                "Turn your rough stems into a cohesive, dynamic mix that translates across all playback systems.",
                Guid.Parse("df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e"),
                "https://open.spotify.com/playlist/37i9dQZF1DZ06evO4pPsgW?si=e069a7940cc7419b",
                350.00,
                u3.IdUser,
                mixingCategory.IdAdvertCategory,
                al1.IdAdvertLog
            );

            var al2 = AdvertLog.Create();
            var a2 = Advert.Create(
                "Professional mixing services by Piotr",
                "Piotr brings 5+ years of mixing expertise in genres ranging from indie rock to electronic dance. He begins each project by analyzing your reference tracks and customizing EQ, compression, and spatial effects to enhance clarity and impact. " +
                "His workflow includes detailed vocal tuning, side-chain ducking for punchy rhythms, and analog emulation for warm, musical saturation. Expect thorough revision rounds and clear communication every step of the way. " +
                "Let Piotr transform your raw sessions into a powerful, polished mix that stands out on streaming platforms and live stages alike.",
                Guid.Parse("17cf17e7-cf1d-4239-ba5a-5f8484191038"),
                "https://open.spotify.com/playlist/37i9dQZF1DX0XUsuxWHRQd?si=123456abcdef",
                400.00,
                u4.IdUser,
                mixingCategory.IdAdvertCategory,
                al2.IdAdvertLog
            );

            var al3 = AdvertLog.Create();
            var a3 = Advert.Create(
                "Mastering expertise by Ewa",
                "Ewa specializes in mastering both digital and analog formats, delivering loudness-optimized masters without sacrificing dynamic range. She uses high-resolution metering and custom multiband compression to sculpt frequencies, tame harshness, and add that final sheen. " +
                "Your track will be delivered in multiple formats (WAV, MP3, DDP) with ISRC embedding and CD-ready files if needed. Ewa also provides detailed EQ and loudness reports so you know exactly how your music will perform on Spotify, Apple Music, and vinyl pressings. " +
                "Bring your mixes to the next level with transparent, professional mastering.",
                Guid.Parse("2de15e61-0ab9-49eb-b5e2-cf909809d22f"),
                "https://open.spotify.com/playlist/37i9dQZF1DWTcaP2wCKa4K?si=abcdef123456",
                300.00,
                u5.IdUser,
                masteringCategory.IdAdvertCategory,
                al3.IdAdvertLog
            );

            var al4 = AdvertLog.Create();
            var a4 = Advert.Create(
                "Full production package from Tomasz",
                "Tomasz offers end-to-end music production: from songwriting support and beat programming to arrangement and mix-ready stems. He crafts custom drum patterns, bass lines, and melodic hooks tailored to your style. " +
                "Using both software synths and hardware outboard gear, he delivers a modern, dynamic sound that stands out in today’s crowded market. Each package includes at least three revision rounds, MIDI files for your own tweaks, and guidance on vocals and performance recording. " +
                "Ideal for solo artists, bands, and labels seeking a cohesive sonic identity.",
                Guid.Parse("cedfe8a0-0a9f-4c4a-a50f-76f9fcac396f"),
                "https://open.spotify.com/playlist/37i9dQZF1DX6T5dcEQpr4L?si=7890abcdef12",
                800.00,
                u6.IdUser,
                productionCategory.IdAdvertCategory,
                al4.IdAdvertLog
            );

            var al5 = AdvertLog.Create();
            var a5 = Advert.Create(
                "Advanced mixing workflows by Michał",
                "Michał combines both in-the-box precision and analog warmth to achieve a balanced, lively mix. He employs gain-riding automation, mid/side processing, and parallel compression to bring out the emotion in your performance. " +
                "With fluency across Pro Tools, Logic Pro, and Ableton Live, he adapts to your session templates and plugin suites seamlessly. You’ll receive detailed session notes, dry/wet stems, and high-resolution WAV master ready for distribution. " +
                "Whether it’s a cinematic score or an underground hip-hop track, Michał’s mixes translate beautifully across car stereos, club systems, and earbuds.",
                Guid.Parse("0c318716-7c49-4735-9ce2-9eb499377e8a"),
                "https://open.spotify.com/playlist/37i9dQZF1DX4FpIdNJcXqW?si=3456abcd7890",
                450.00,
                u7.IdUser,
                mixingCategory.IdAdvertCategory,
                al5.IdAdvertLog
            );

            var al6 = AdvertLog.Create();
            var a6 = Advert.Create(
                "Mastering for vinyl & streaming—Katarzyna",
                "Katarzyna offers specialized mastering for both vinyl pressings and digital platforms. She carefully sequences tracks, applies EQ to prevent low-end overmodulation, and optimizes side-chain compression for needle-friendly dynamics. " +
                "For streaming masters, she fine-tunes loudness to meet platform standards (Spotify, Apple Music, Tidal) while preserving headroom and musicality. You’ll get final masters in DDP, WAV, and MP3 formats, plus an analytical Loudness Unit Full Scale (LUFS) report. " +
                "Elevate your project with a mastering engineer who understands the nuances of different playback mediums.",
                Guid.Parse("504cd9ce-5804-4f76-b6bf-706aae87a1b0"),
                "https://open.spotify.com/playlist/37i9dQZF1DX4WYpdgoIcn6?si=abcd3456ef90",
                320.00,
                u8.IdUser,
                masteringCategory.IdAdvertCategory,
                al6.IdAdvertLog
            );

            var al7 = AdvertLog.Create();
            var a7 = Advert.Create(
                "Beat production & stems by Krzysztof",
                "Krzysztof specializes in crafting genre-blending beats—from trap and lo-fi to funk and soul. Each beat comes with full MIDI programming, drum samples, and multitrack stems so you can rearrange or remix at will. " +
                "He also offers vocal comping and editing as an add-on, ensuring your performance sits perfectly in the groove. Expect high-quality WAVs, labeled session files, and a quick turnaround. " +
                "Perfect for rappers, singers, and producers looking for fresh, customizable sound beds.",
                Guid.Parse("ef11919d-3e86-4c08-a594-03800f613fd8"),
                "https://open.spotify.com/playlist/37i9dQZF1DX6K6802AIa8E?si=ef1234567890",
                900.00,
                u9.IdUser,
                productionCategory.IdAdvertCategory,
                al7.IdAdvertLog
            );

            var al8 = AdvertLog.Create();
            var a8 = Advert.Create(
                "Mix engineering—Agnieszka’s precision approach",
                "Agnieszka takes a surgical approach to mixing: corrective EQ, transparent compression, and creative spatial effects that serve your song. She communicates clearly, providing time-stamped revision notes and A/B comparisons. " +
                "Using analog summing and high-end outboard gear, she injects warmth and depth, then returns to the box for final automation rides. You receive both instrumental and vocal stems plus a mastered reference for quick upload. " +
                "Ideal for artists who demand both technical accuracy and emotional impact in their mixes.",
                Guid.Parse("d98dd161-cd11-4397-b6b2-48a5656c20a3"),
                "https://open.spotify.com/playlist/37i9dQZF1DWXRqgorJj26U?si=4567abcd1234",
                480.00,
                u10.IdUser,
                mixingCategory.IdAdvertCategory,
                al8.IdAdvertLog
            );

            var al9 = AdvertLog.Create();
            var a9 = Advert.Create(
                "Mastering & delivery by Paweł",
                "Paweł provides a full delivery package: mastered WAV, high-quality MP3, and separated stems for remixers or video post-production. He focuses on dynamic control, spectral balance, and proper headroom for broadcast. " +
                "He also embeds metadata (ISRC, artist name, album art) so you can deliver directly to digital distributors with confidence. Comprehensive test masters are supplied so you can preview on headphones, car, and club systems. " +
                "Get radio-ready masters that truly represent your artistic vision.",
                Guid.Parse("c2363242-295c-4435-867c-90d9b96b085a"),
                "https://open.spotify.com/playlist/37i9dQZF1DWY4xHQp97fN6?si=bcdef7890123",
                350.00,
                u11.IdUser,
                masteringCategory.IdAdvertCategory,
                al9.IdAdvertLog
            );

            modelBuilder.Entity<AdvertLog>().HasData(
                al1, al2, al3, al4, al5, al6, al7, al8, al9
            );

            modelBuilder.Entity<Advert>().HasData(
                a1, a2, a3, a4, a5, a6, a7, a8, a9
            );
        }*/
    }
}