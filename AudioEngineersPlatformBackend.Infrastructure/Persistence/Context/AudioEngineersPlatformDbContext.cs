using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;

public class AudioEngineersPlatformDbContext : DbContext
{
    public AudioEngineersPlatformDbContext(
        DbContextOptions<AudioEngineersPlatformDbContext> options
    ) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserAuthLog> UserAuthLogs { get; set; }
    public virtual DbSet<Token> Tokens { get; set; }
    public virtual DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
    public virtual DbSet<SocialMediaName> SocialMediaNames { get; set; }
    public virtual DbSet<Advert> Adverts { get; set; }
    public virtual DbSet<AdvertCategory> AdvertCategories { get; set; }
    public virtual DbSet<AdvertLog> AdvertLogs { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<ReviewLog> ReviewLogs { get; set; }
    public virtual DbSet<UserMessage> UserMessages { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<HubConnection> HubConnections { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        // Apply configurations from the assembly.
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AudioEngineersPlatformDbContext).Assembly);

        // Data seeding for the database.

        // Seed Role Entity.
        Role adminRole = Role.CreateWithId(Guid.Parse("D92D29B8-F462-46DF-8EFB-DE6B9AA5109A"), "Administrator");

        Role clientRole = Role.CreateWithId(Guid.Parse("004865E2-177F-4C54-BB4C-69799F0BF315"), "Client");

        Role audioEngineerRole =
            Role.CreateWithId(Guid.Parse("522C6700-165E-4189-B234-9FB533266E07"), "Audio engineer");

        modelBuilder
            .Entity<Role>()
            .HasData
            (
                adminRole,
                clientRole,
                audioEngineerRole
            );

        // Seed UserAuthLog with User Entities (mock users).
        // A precomputed hash for "test" password.
        const string precomputedPasswordHash =
            "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==";

        // UserAuthLog ul0 = UserAuthLog.CreateWithIdAndStaticData
        // (
        //     Guid.Parse("464433C3-7796-49EE-9EB1-C0818F98A329"),
        //     new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        // );
        // User u0 = User.CreateWithId
        // (
        //     Guid.Parse("FCA13465-F8E6-4FBD-8ED4-2644294ED215"),
        //     "Stanisław",
        //     "Stepka",
        //     "s28016@pjwstk.edu.pl",
        //     "+48696784867",
        //     "test",
        //     audioEngineerRole.IdRole,
        //     ul0.IdUserAuthLog
        // );
        // u0.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul1 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("5CB8EFAA-2432-46D1-9984-B41A40BAB7B3"),
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        );
        ul1.SetIsVerifiedStatus(true);

        User u1 = User.CreateWithId
        (
            Guid.Parse("AEBC2724-0EDF-4691-99E9-65CBD3AAB3BF"),
            "Dominik",
            "Kowalski",
            "dominik.kow@gmail.com",
            "+48123456789",
            "test",
            adminRole.IdRole,
            ul1.IdUserAuthLog
        );
        u1.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul2 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("2F765163-6728-48BC-9767-66687EFDF86E"),
            new DateTime(2025, 3, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        ul2.SetIsVerifiedStatus(true);

        User u2 = User.CreateWithId
        (
            Guid.Parse("5BFC9C8D-4789-4065-99D9-81EC5B58C0F5"),
            "Jan",
            "Nowak",
            "jan.nowak@gmail.com",
            "+48696432123",
            "test",
            clientRole.IdRole,
            ul2.IdUserAuthLog
        );
        u2.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul3 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("8312D4FD-FE6D-4001-A037-CDE12000161D"),
            new DateTime(2025, 3, 11, 0, 0, 0, DateTimeKind.Utc)
        );
        ul3.SetIsVerifiedStatus(true);

        User u3 = User.CreateWithId
        (
            Guid.Parse("828DAA53-9A49-40AD-97B3-31B0349BC08D"),
            "Anna",
            "Kowalska",
            "anna.kow@gmail.com",
            "+48543123123",
            "test",
            audioEngineerRole.IdRole,
            ul3.IdUserAuthLog
        );
        u3.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul4 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("E7653083-1497-4AA0-A56B-DEC32A61D71F"),
            new DateTime(2024, 1, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        ul4.SetIsVerifiedStatus(true);

        User u4 = User.CreateWithId
        (
            Guid.Parse("2254933A-66AC-4AB8-A923-25D508D8B5C0"),
            "Piotr",
            "Nowak",
            "piotr.nowak@gmail.com",
            "+48111222333",
            "test",
            audioEngineerRole.IdRole,
            ul4.IdUserAuthLog
        );
        u4.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul5 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("9AE2C2F3-4AB1-4512-9832-7649D5FF61D8"),
            new DateTime(2023, 12, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        ul5.SetIsVerifiedStatus(true);

        User u5 = User.CreateWithId
        (
            Guid.Parse("731C7617-9342-415D-8E06-F77EC2D56786"),
            "Ewa",
            "Maj",
            "ewa.maj@gmail.com",
            "+48111333444",
            "test",
            audioEngineerRole.IdRole,
            ul5.IdUserAuthLog
        );
        u5.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul6 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("5091BF83-DF7D-4A54-A35B-31B44D1A1643"),
            new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc)
        );
        ul6.SetIsVerifiedStatus(true);

        User u6 = User.CreateWithId
        (
            Guid.Parse("29D1D9BD-87D9-4125-99A5-0F15C9DF3A30"),
            "Tomasz",
            "Zieliński",
            "tomasz.zielinski@gmail.com",
            "+48111444555",
            "test",
            audioEngineerRole.IdRole,
            ul6.IdUserAuthLog
        );
        u6.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul7 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("DF0A8813-0938-42A6-AC84-26298701F456"),
            new DateTime(2023, 12, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        ul7.SetIsVerifiedStatus(true);

        User u7 = User.CreateWithId
        (
            Guid.Parse("3FB9E066-38B7-42AE-900C-D7AB5AE280F0"),
            "Michał",
            "Wójcik",
            "michal.wojcik@gmail.com",
            "+48111555666",
            "test",
            audioEngineerRole.IdRole,
            ul7.IdUserAuthLog
        );
        u7.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul8 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("2A019DC8-FE9F-4A63-B692-49E03F889F7F"),
            new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)
        );
        ul8.SetIsVerifiedStatus(true);

        User u8 = User.CreateWithId
        (
            Guid.Parse("AC89F1A4-6988-4211-8136-FBF9B45E4CF2"),
            "Katarzyna",
            "Wiśniewska",
            "katarzyna.wisniewska@gmail.com",
            "+48111666777",
            "test",
            audioEngineerRole.IdRole,
            ul8.IdUserAuthLog
        );
        u8.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul9 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("C91C99CA-FFFD-42A5-9E6E-FA67D3C0F762"),
            new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc)
        );
        ul9.SetIsVerifiedStatus(true);

        User u9 = User.CreateWithId
        (
            Guid.Parse("07434FD4-3450-4A01-A8C4-C371ED011E48"),
            "Krzysztof",
            "Lewandowski",
            "krzysztof.lewandowski@gmail.com",
            "+48111777888",
            "test",
            audioEngineerRole.IdRole,
            ul9.IdUserAuthLog
        );
        u9.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul10 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("8DB9E713-D6F0-4F34-B348-C7DA0C1A51D6"),
            new DateTime(2024, 2, 3, 0, 0, 0, DateTimeKind.Utc)
        );
        ul10.SetIsVerifiedStatus(true);

        User u10 = User.CreateWithId
        (
            Guid.Parse("E07BC534-3324-4AF4-8D97-FAEE7242E896"),
            "Agnieszka",
            "Wróbel",
            "agnieszka.wrobel@gmail.com",
            "+48111888999",
            "test",
            audioEngineerRole.IdRole,
            ul10.IdUserAuthLog
        );
        u10.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul11 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("32AFFE63-9BB3-4C86-BBF8-6D5E37C7FB3F"),
            new DateTime(2023, 12, 3, 0, 0, 0, DateTimeKind.Utc)
        );
        ul11.SetIsVerifiedStatus(true);

        User u11 = User.CreateWithId
        (
            Guid.Parse("1D31A511-8D38-4223-96A0-F2B15CC90794"),
            "Paweł",
            "Kamiński",
            "pawel.kaminski@gmail.com",
            "+48111999000",
            "test",
            audioEngineerRole.IdRole,
            ul11.IdUserAuthLog
        );
        u11.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul12 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("CD9E4F1F-8EDD-4488-B0DA-256521A720E8"),
            new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
        );
        ul12.SetIsVerifiedStatus(true);

        User u12 = User.CreateWithId
        (
            Guid.Parse("655887CB-B3CD-40DA-B2BB-48B5E84239F9"),
            "Marcin",
            "Radwański",
            "mar.radw@gmail.com",
            "+48431234765",
            "test",
            adminRole.IdRole,
            ul12.IdUserAuthLog
        );
        u12.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul13 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("B0F3E786-F68B-46FE-8B18-F4A6E1150804"),
            new DateTime(2024, 4, 4, 0, 0, 0, DateTimeKind.Utc)
        );
        ul13.SetIsVerifiedStatus(true);

        User u13 = User.CreateWithId
        (
            Guid.Parse("FDF7BDA4-F40F-484F-BC40-ADBF8AA98985"),
            "Marian",
            "Niewiadomski",
            "marian@gmail.com",
            "+48654123432",
            "test",
            clientRole.IdRole,
            ul13.IdUserAuthLog
        );
        u13.SetHashedPassword(precomputedPasswordHash);

        UserAuthLog ul14 = UserAuthLog.CreateWithIdAndStaticData
        (
            Guid.Parse("DBF24F67-7457-47C3-A2AF-A117D8E90B00"),
            new DateTime(2025, 3, 23, 0, 0, 0, DateTimeKind.Utc)
        );
        ul14.SetIsVerifiedStatus(true);

        User u14 = User.CreateWithId
        (
            Guid.Parse("156765B0-84A0-4389-AF75-78F2F36DEA04"),
            "Maria",
            "Dąbrowska",
            "dab@gmail.com",
            "+48231443225",
            "test",
            clientRole.IdRole,
            ul14.IdUserAuthLog
        );
        u14.SetHashedPassword(precomputedPasswordHash);

        modelBuilder
            .Entity<UserAuthLog>()
            .HasData
            (
                // ul0,
                ul1,
                ul2,
                ul3,
                ul4,
                ul5,
                ul6,
                ul7,
                ul8,
                ul9,
                ul10,
                ul11,
                ul12,
                ul13,
                ul14
            );

        modelBuilder
            .Entity<User>()
            .HasData
            (
                // u0,
                u1,
                u2,
                u3,
                u4,
                u5,
                u6,
                u7,
                u8,
                u9,
                u10,
                u11,
                u12,
                u13,
                u14
            );

        // Seed AdvertCategory Entity.
        AdvertCategory mixingCategory =
            AdvertCategory.CreateWithId(Guid.Parse("E6DDD487-8B56-4C8F-B289-2F04BABBABDA"), "Mixing");
        AdvertCategory masteringCategory =
            AdvertCategory.CreateWithId(Guid.Parse("80C20081-C580-4AAF-A346-2587CCFDEBF5"), "Mastering");
        AdvertCategory productionCategory =
            AdvertCategory.CreateWithId(Guid.Parse("B8785564-E008-4889-B633-7F5D3558EB92"), "Production");

        modelBuilder
            .Entity<AdvertCategory>()
            .HasData
            (
                mixingCategory,
                masteringCategory,
                productionCategory
            );

        // Seed the AdvertLog and Advert Entities.
        AdvertLog al1 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("1B84601E-E225-4E9D-93D2-911FB0A1569E"),
            new DateTime(2024, 2, 3, 12, 35, 0, DateTimeKind.Utc)
        );
        Advert a1 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("31BA89AA-F10F-40E7-B4B0-7375DA567997"),
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

        AdvertLog al2 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("A9E9762A-2A67-46F3-B371-50405A100D58"),
            new DateTime(2024, 5, 14, 15, 51, 0, DateTimeKind.Utc)
        );
        Advert a2 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("AFF251D8-9E58-4F5C-BA43-4C6597FC8A08"),
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

        AdvertLog al3 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("B3D2DCE1-A858-4312-937D-C56A6E0178CF"),
            new DateTime(2025, 2, 6, 17, 35, 0, DateTimeKind.Utc)
        );
        Advert a3 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("7BFD7CFA-5FDE-42E2-AC56-9EE1040B708F"),
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

        AdvertLog al4 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("EFE85186-52C9-4C46-B585-D4B47523DB47"),
            new DateTime(2025, 5, 3, 12, 5, 0, DateTimeKind.Utc)
        );
        Advert a4 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("8370E2EB-2EA0-4C4E-99E5-B9E719427F03"),
            "Full production package from Tomasz",
            "Tomasz offers end-to-end music production: from songwriting support and beat programming to arrangement and mix-ready stems. He crafts custom drum patterns, bass lines, and melodic hooks tailored to your style. " +
            "Using both software synths and hardware outboard gear, he delivers a modern, dynamic sound that stands out in today’s crowded market. Each package includes at least three revision rounds, MIDI files for your own tweaks, and guidance on vocals and performance recording. " +
            "Ideal for solo artists, bands, and labels seeking a cohesive sonic identity.",
            Guid.Parse("cedfe8a0-0a9f-4c4a-a50f-76f9fcac396f"),
            "https://open.spotify.com/playlist/4nZo2X8iHrwhYBYdKvysgI",
            800.00,
            u6.IdUser,
            productionCategory.IdAdvertCategory,
            al4.IdAdvertLog
        );

        AdvertLog al5 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("24BA6029-F88C-4B12-9A63-BF00C2D9F3E4"),
            new DateTime(2026, 6, 12, 9, 23, 0, DateTimeKind.Utc)
        );
        Advert a5 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("8CB96D43-A8A9-4010-8613-F721ECEDB8B3"),
            "Advanced mixing workflows by Michał",
            "Michał combines both in-the-box precision and analog warmth to achieve a balanced, lively mix. He employs gain-riding automation, mid/side processing, and parallel compression to bring out the emotion in your performance. " +
            "With fluency across Pro Tools, Logic Pro, and Ableton Live, he adapts to your session templates and plugin suites seamlessly. You’ll receive detailed session notes, dry/wet stems, and high-resolution WAV master ready for distribution. " +
            "Whether it’s a cinematic score or an underground hip-hop track, Michał’s mixes translate beautifully across car stereos, club systems, and earbuds.",
            Guid.Parse("0c318716-7c49-4735-9ce2-9eb499377e8a"),
            "https://open.spotify.com/playlist/3eoncc59w7c8t1PnKtSOh6",
            450.00,
            u7.IdUser,
            mixingCategory.IdAdvertCategory,
            al5.IdAdvertLog
        );

        AdvertLog al6 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("FE0D1832-793E-4CF8-983A-BBE09D7E0FA2"),
            new DateTime(2025, 6, 23, 22, 25, 0, DateTimeKind.Utc)
        );
        Advert a6 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("18809BE2-B063-4ADA-A7A4-81F9FA107322"),
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

        AdvertLog al7 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("993648CA-9D51-419A-85E8-046E8FC3162B"),
            new DateTime(2025, 6, 13, 23, 12, 0, DateTimeKind.Utc)
        );
        Advert a7 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("72AC8A29-19E2-4B7B-B810-418D638B5356"),
            "Beat production & stems by Krzysztof",
            "Krzysztof specializes in crafting genre-blending beats—from trap and lo-fi to funk and soul. Each beat comes with full MIDI programming, drum samples, and multitrack stems so you can rearrange or remix at will. " +
            "He also offers vocal comping and editing as an add-on, ensuring your performance sits perfectly in the groove. Expect high-quality WAVs, labeled session files, and a quick turnaround. " +
            "Perfect for rappers, singers, and producers looking for fresh, customizable sound beds.",
            Guid.Parse("ef11919d-3e86-4c08-a594-03800f613fd8"),
            "https://open.spotify.com/playlist/2UZk7JjJnbTut1w8fqs3JL",
            900.00,
            u9.IdUser,
            productionCategory.IdAdvertCategory,
            al7.IdAdvertLog
        );

        AdvertLog al8 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("C8AB7E20-E7DD-4616-8862-D15DAD3C986A"),
            new DateTime(2025, 4, 3, 12, 11, 0, DateTimeKind.Utc)
        );
        Advert a8 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("33545021-1FFB-4F46-9DF8-6242B8F0786F"),
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

        AdvertLog al9 = AdvertLog.CreateWithIdAndStaticData
        (
            Guid.Parse("70820368-D390-4C01-AF1F-9E7B8E8413D2"),
            new DateTime(2025, 5, 13, 6, 22, 0, DateTimeKind.Utc)
        );
        Advert a9 = Advert.CreateWithIdAndStaticData
        (
            Guid.Parse("A79C87D0-276B-48BD-B23C-9AF67AFD4C41"),
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

        modelBuilder
            .Entity<AdvertLog>()
            .HasData
            (
                al1,
                al2,
                al3,
                al4,
                al5,
                al6,
                al7,
                al8,
                al9
            );

        modelBuilder
            .Entity<Advert>()
            .HasData
            (
                a1,
                a2,
                a3,
                a4,
                a5,
                a6,
                a7,
                a8,
                a9
            );

        // Seed ReviewLog Entity.
        ReviewLog rl1 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("D9DE48FD-0ABC-4B52-8371-F9F6959FDC46"),
            new DateTime(2025, 3, 13, 15, 48, 0, DateTimeKind.Utc)
        );

        ReviewLog rl2 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("1F642C35-DBFB-4062-AE75-7CF0E3F27F6F"),
            new DateTime(2025, 5, 7, 13, 23, 0, DateTimeKind.Utc)
        );

        ReviewLog rl3 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("9473BB77-CFF3-42D7-BD0E-2807AA2FEF52"),
            new DateTime(2025, 4, 17, 22, 21, 0, DateTimeKind.Utc)
        );

        ReviewLog rl4 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("0D38BC51-218B-4E12-8E39-6BEF8654419B"),
            new DateTime(2025, 4, 29, 21, 17, 0, DateTimeKind.Utc)
        );

        ReviewLog rl5 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("39CC7997-692F-40F4-A3EC-68B00940F6A6"),
            new DateTime(2025, 2, 16, 13, 11, 0, DateTimeKind.Utc)
        );

        ReviewLog rl6 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("60BB7AC9-88DE-4BD4-933B-CE3E71D9CB45"),
            new DateTime(2025, 5, 26, 15, 42, 0, DateTimeKind.Utc)
        );

        ReviewLog rl7 = ReviewLog.CreateWithIdAndStaticData
        (
            Guid.Parse("3461A295-B612-4526-AAF1-205EA3A6BEFF"),
            new DateTime(2025, 4, 24, 19, 30, 0, DateTimeKind.Utc)
        );

        modelBuilder
            .Entity<ReviewLog>()
            .HasData
            (
                rl1,
                rl2,
                rl3,
                rl4,
                rl5,
                rl6,
                rl7
            );

        // Seed Review Entity.
        Review r1 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("DBE3112C-8914-44B1-8011-D58CB2BA4270"),
            a7.IdAdvert,
            rl1.IdReviewLog,
            u2.IdUser,
            "I feel like the engineer could not really achieve what I have wanted, however I think they were really patient and creative ;)",
            3
        );

        Review r2 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("F88CB211-6464-4B28-AA48-75F257624D86"),
            a7.IdAdvert,
            rl2.IdReviewLog,
            u13.IdUser,
            "Excellent mixes, I have never worked with such a talented engineer in my life. I will recommend working with him all the way!",
            5
        );

        Review r3 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("649406C7-A59A-4102-B7CB-C39D16BC7117"),
            a2.IdAdvert,
            rl3.IdReviewLog,
            u14.IdUser,
            "They way I was treated was great. I will most definitely visit this engineer in the studio again!",
            5
        );

        Review r4 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("FBD98612-7CCD-4B83-AFAA-7084E758E746"),
            a2.IdAdvert,
            rl4.IdReviewLog,
            u12.IdUser,
            "Poor judgement, does not understand the music I like and hates on doing some additional revisions of a master, I can't recommend this person...",
            1
        );

        Review r5 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("2D7FE610-9FB0-4A8E-923C-2F7A8AFE2A78"),
            a3.IdAdvert,
            rl5.IdReviewLog,
            u13.IdUser,
            "I feel like this person knows their craft and is capable of delivering a good mix. I can most definitely recommend working with them :)",
            4
        );

        Review r6 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("5F3BCC4B-D484-44CC-BAA2-339373B7D0F0"),
            a4.IdAdvert,
            rl6.IdReviewLog,
            u2.IdUser,
            "Great production skills, I am very happy with the final result!",
            5
        );

        Review r7 = Review.CreateWithIdAndStaticData
        (
            Guid.Parse("A1B2C3D4-E5F6-7A8B-9C0D-E1F2A3B4C5D6"),
            a1.IdAdvert,
            rl7.IdReviewLog,
            u14.IdUser,
            "The mix was good, but I expected more attention to detail. Overall, a decent experience.",
            3
        );

        modelBuilder
            .Entity<Review>()
            .HasData
            (
                r1,
                r2,
                r3,
                r4,
                r5,
                r6,
                r7
            );

        // Seed SocialMediaName Entity.
        SocialMediaName smn1 = SocialMediaName.CreateWithId
        (
            Guid.Parse("02C8722F-DCCC-4060-BEC3-C95815C67703"),
            "Instagram"
        );

        SocialMediaName smn2 = SocialMediaName.CreateWithId
        (
            Guid.Parse("371DBD6D-76EB-4266-AAA3-2B431C5CBAFE"),
            "Facebook"
        );

        SocialMediaName smn3 = SocialMediaName.CreateWithId
        (
            Guid.Parse("4639C978-26FC-4027-B036-3FC5C0D1D221"),
            "Linkedin"
        );

        modelBuilder
            .Entity<SocialMediaName>()
            .HasData
            (
                smn1,
                smn2,
                smn3
            );

        // Seed SocialMediaLink Entity.
        SocialMediaLink sml1 = SocialMediaLink.CreateWithId
        (
            Guid.Parse("8C0D3528-E2CB-430A-BFD4-8E0623C714CF"),
            u3.IdUser,
            "https://www.instagram.com/prod.mustang/",
            smn1.IdSocialMediaName
        );

        SocialMediaLink sml2 = SocialMediaLink.CreateWithId
        (
            Guid.Parse("7667E3A7-E8F9-4049-AF10-A0A405DACF40"),
            u3.IdUser,
            "https://www.facebook.com/prod.mustangg/",
            smn2.IdSocialMediaName
        );

        SocialMediaLink sml3 = SocialMediaLink.CreateWithId
        (
            Guid.Parse("B5B570DD-43C8-471E-976B-91A0D50DE9F5"),
            u3.IdUser,
            "https://www.linkedin.com/in/stanisław-stepka/",
            smn3.IdSocialMediaName
        );

        modelBuilder
            .Entity<SocialMediaLink>()
            .HasData
            (
                sml1,
                sml2,
                sml3
            );
    }
}