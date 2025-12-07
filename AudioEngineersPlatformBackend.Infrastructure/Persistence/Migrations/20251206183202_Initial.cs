using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AudioEngineersPlatformBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertCategory",
                columns: table => new
                {
                    IdAdvertCategory = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertCategory", x => x.IdAdvertCategory);
                });

            migrationBuilder.CreateTable(
                name: "AdvertLog",
                columns: table => new
                {
                    IdAdvertLog = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertLog", x => x.IdAdvertLog);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    IdMessage = table.Column<Guid>(type: "uuid", nullable: false),
                    TextContent = table.Column<string>(type: "text", nullable: true),
                    FileKey = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    DateSent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.IdMessage);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLog",
                columns: table => new
                {
                    IdReviewLog = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLog", x => x.IdReviewLog);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaName",
                columns: table => new
                {
                    IdSocialMediaName = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaName", x => x.IdSocialMediaName);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthLog",
                columns: table => new
                {
                    IdUserAuthLog = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsResettingEmail = table.Column<bool>(type: "boolean", nullable: false),
                    IsResettingPassword = table.Column<bool>(type: "boolean", nullable: false),
                    IsRemindingPassword = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthLog", x => x.IdUserAuthLog);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IdRole = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUserAuthLog = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.IdRole,
                        principalTable: "Role",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_UserLog",
                        column: x => x.IdUserAuthLog,
                        principalTable: "UserAuthLog",
                        principalColumn: "IdUserAuthLog",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Advert",
                columns: table => new
                {
                    IdAdvert = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CoverImageKey = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioUrl = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAdvertCategory = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAdvertLog = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advert", x => x.IdAdvert);
                    table.ForeignKey(
                        name: "FK_Advert_AdvertCategory",
                        column: x => x.IdAdvertCategory,
                        principalTable: "AdvertCategory",
                        principalColumn: "IdAdvertCategory");
                    table.ForeignKey(
                        name: "FK_Advert_AdvertLog",
                        column: x => x.IdAdvertLog,
                        principalTable: "AdvertLog",
                        principalColumn: "IdAdvertLog",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advert_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HubConnection",
                columns: table => new
                {
                    IdHubConnection = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connection", x => x.IdHubConnection);
                    table.ForeignKey(
                        name: "FK_HubConnection_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaLink",
                columns: table => new
                {
                    IdSocialMediaLink = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSocialMediaName = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaLink", x => x.IdSocialMediaLink);
                    table.ForeignKey(
                        name: "FK_SocialMediaLink_SocialMediaName",
                        column: x => x.IdSocialMediaName,
                        principalTable: "SocialMediaName",
                        principalColumn: "IdSocialMediaName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SocialMediaLink_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    IdToken = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.IdToken);
                    table.ForeignKey(
                        name: "FK_User_Token",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMessage",
                columns: table => new
                {
                    IdUserMessage = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUserSender = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IdUserRecipient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMessage = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessage", x => x.IdUserMessage);
                    table.ForeignKey(
                        name: "FK_UserMessage_Message",
                        column: x => x.IdMessage,
                        principalTable: "Message",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMessage_UserRecipient",
                        column: x => x.IdUserRecipient,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMessage_UserSender",
                        column: x => x.IdUserSender,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    IdReview = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SatisfactionLevel = table.Column<byte>(type: "smallint", nullable: false),
                    IdAdvert = table.Column<Guid>(type: "uuid", nullable: false),
                    IdReviewLog = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.IdReview);
                    table.ForeignKey(
                        name: "FK_Review_Advert",
                        column: x => x.IdAdvert,
                        principalTable: "Advert",
                        principalColumn: "IdAdvert",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_ReviewLog",
                        column: x => x.IdReviewLog,
                        principalTable: "ReviewLog",
                        principalColumn: "IdReviewLog",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AdvertCategory",
                columns: new[] { "IdAdvertCategory", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), "Mastering" },
                    { new Guid("b8785564-e008-4889-b633-7f5d3558eb92"), "Production" },
                    { new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), "Mixing" }
                });

            migrationBuilder.InsertData(
                table: "AdvertLog",
                columns: new[] { "IdAdvertLog", "DateCreated", "DateDeleted", "DateModified", "IsActive", "IsDeleted" },
                values: new object[,]
                {
                    { new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"), new DateTime(2024, 2, 3, 12, 35, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"), new DateTime(2025, 6, 12, 9, 23, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"), new DateTime(2025, 5, 13, 6, 22, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"), new DateTime(2025, 8, 13, 23, 12, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"), new DateTime(2024, 5, 14, 15, 51, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"), new DateTime(2025, 2, 6, 17, 35, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"), new DateTime(2025, 9, 3, 12, 11, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("efe85186-52c9-4c46-b585-d4b47523db47"), new DateTime(2025, 5, 3, 12, 5, 0, 0, DateTimeKind.Utc), null, null, true, false },
                    { new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"), new DateTime(2025, 6, 23, 22, 25, 0, 0, DateTimeKind.Utc), null, null, true, false }
                });

            migrationBuilder.InsertData(
                table: "ReviewLog",
                columns: new[] { "IdReviewLog", "DateCreated", "DateDeleted", "IsDeleted" },
                values: new object[,]
                {
                    { new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"), new DateTime(2025, 4, 29, 21, 17, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"), new DateTime(2025, 5, 7, 13, 23, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"), new DateTime(2025, 4, 24, 19, 30, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"), new DateTime(2025, 2, 16, 13, 11, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"), new DateTime(2025, 5, 26, 15, 42, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"), new DateTime(2025, 4, 17, 22, 21, 0, 0, DateTimeKind.Utc), null, false },
                    { new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"), new DateTime(2025, 3, 13, 15, 48, 0, 0, DateTimeKind.Utc), null, false }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "IdRole", "RoleName" },
                values: new object[,]
                {
                    { new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), "Client" },
                    { new Guid("522c6700-165e-4189-b234-9fb533266e07"), "Audio engineer" },
                    { new Guid("d92d29b8-f462-46df-8efb-de6b9aa5109a"), "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "SocialMediaName",
                columns: new[] { "IdSocialMediaName", "Name" },
                values: new object[,]
                {
                    { new Guid("02c8722f-dccc-4060-bec3-c95815c67703"), "Instagram" },
                    { new Guid("371dbd6d-76eb-4266-aaa3-2b431c5cbafe"), "Facebook" },
                    { new Guid("4639c978-26fc-4027-b036-3fc5c0d1d221"), "Linkedin" }
                });

            migrationBuilder.InsertData(
                table: "UserAuthLog",
                columns: new[] { "IdUserAuthLog", "DateCreated", "DateDeleted", "DateLastLogin", "IsDeleted", "IsRemindingPassword", "IsResettingEmail", "IsResettingPassword", "IsVerified" },
                values: new object[,]
                {
                    { new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"), new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("2f765163-6728-48bc-9767-66687efdf86e"), new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"), new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"), new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"), new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"), new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"), new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"), new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"), new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("df0a8813-0938-42a6-ac84-26298701f456"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true },
                    { new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"), new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, false, false, false, true }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "IdUser", "Email", "FirstName", "IdRole", "IdUserAuthLog", "LastName", "Password", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"), "krzysztof.lewandowski@gmail.com", "Krzysztof", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"), "Lewandowski", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111777888" },
                    { new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), "dab@gmail.com", "Maria", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"), "Dąbrowska", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48231443225" },
                    { new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"), "pawel.kaminski@gmail.com", "Paweł", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"), "Kamiński", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111999000" },
                    { new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"), "piotr.nowak@gmail.com", "Piotr", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"), "Nowak", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111222333" },
                    { new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"), "tomasz.zielinski@gmail.com", "Tomasz", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"), "Zieliński", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111444555" },
                    { new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"), "michal.wojcik@gmail.com", "Michał", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("df0a8813-0938-42a6-ac84-26298701f456"), "Wójcik", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111555666" },
                    { new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), "jan.nowak@gmail.com", "Jan", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("2f765163-6728-48bc-9767-66687efdf86e"), "Nowak", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48696432123" },
                    { new Guid("655887cb-b3cd-40da-b2bb-48b5e84239f9"), "mar.radw@gmail.com", "Marcin", new Guid("d92d29b8-f462-46df-8efb-de6b9aa5109a"), new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"), "Radwański", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48431234765" },
                    { new Guid("731c7617-9342-415d-8e06-f77ec2d56786"), "ewa.maj@gmail.com", "Ewa", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"), "Maj", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111333444" },
                    { new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "anna.kow@gmail.com", "Anna", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"), "Kowalska", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48543123123" },
                    { new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"), "katarzyna.wisniewska@gmail.com", "Katarzyna", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"), "Wiśniewska", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111666777" },
                    { new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"), "dominik.kow@gmail.com", "Dominik", new Guid("d92d29b8-f462-46df-8efb-de6b9aa5109a"), new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"), "Kowalski", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48123456789" },
                    { new Guid("e07bc534-3324-4af4-8d97-faee7242e896"), "agnieszka.wrobel@gmail.com", "Agnieszka", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"), "Wróbel", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48111888999" },
                    { new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), "marian@gmail.com", "Marian", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"), "Niewiadomski", "AQAAAAIAAYagAAAAEFIagC5C9vbbJvt0Laj4EFwEie4imyDjDa7Ug56CJY8hKm9ftbEdPRtKo/dXKkW3cQ==", "+48654123432" }
                });

            migrationBuilder.InsertData(
                table: "Advert",
                columns: new[] { "IdAdvert", "CoverImageKey", "Description", "IdAdvertCategory", "IdAdvertLog", "IdUser", "PortfolioUrl", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("18809be2-b063-4ada-a7a4-81f9fa107322"), new Guid("504cd9ce-5804-4f76-b6bf-706aae87a1b0"), "Cześć jestem Kasia i masteruje utwory. Miałeś kiedyś tak, że po wrzuceniu utworu na platformy streamingowe był cichy? Nie uderzał tak mocno jak inne utwory? To kwestia masteringu, albo jego braku! Jeśli nie wiesz co zrobić, to wystarczy, że do mnie napiszesz, prześlesz swoje pliki, a ja powiem Tobie co jest nie tak - dobry mastering to nie tylko kwestia dobrego masteringowca, ale też dobrego miksu. Gdyby coś było nie tak, odrazu dam Tobie znać, bo chodzi o to, żebyś miał najlepszy efekt finalny!", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"), new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"), "https://open.spotify.com/playlist/37i9dQZF1DX4WYpdgoIcn6?si=abcd3456ef90", 320.0, "Najgłośniejsze mastery" },
                    { new Guid("31ba89aa-f10f-40e7-b4b0-7375da567997"), new Guid("df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e"), "Mam ponad 15 lat doświadczenia z miksowaniem utworów. Pracowałam nad największymi hitami Polskich artystów oraz z największymi gwiazdami branży audio. Wyróżniam się tym, że podchodzę indywidualnie do każdego zlecenia.Korzystam z najlepszych narzędzi oraz sprzętu analogowego. To właśnie w ten sposób mogę zapewnić Tobie świetne brzmienie w bezkonkurencyjnej cenie! Jeśli szukasz najlepszego inżyniera, to dobrze trafiłeś.Spowoduje, że nawet najgorzej brzmiące nagrania, będą brzmiały jak ze studio za milion dolarów :)", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"), new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "https://open.spotify.com/playlist/37i9dQZF1DZ06evO4pPsgW?si=e069a7940cc7419b", 350.0, "Zmiksuję twój utwór za bezcen!" },
                    { new Guid("33545021-1ffb-4f46-9df8-6242b8f0786f"), new Guid("d98dd161-cd11-4397-b6b2-48a5656c20a3"), "Cześć, jestem inżynierem audio, który dobrze wie, co zrobić, żeby twój utwór brzmiał najlepiej. Pracuje tylko z najlepszymi emulacjami sprzętu analogowego (UAD, Slate i Waves) i bardzo cenie sobie dobrą organizację pracy. Preferuje pracę z klientami, którzy potrafią mi trafnie wypunktować poprawki do utworów, nad którymi pracujemy, dlatego proszę też, abyś liczył się z tym, że nie będę z Tobą pracować, jeśli nie będziesz potrafił/a tego zaakceptować. Jeśli szukasz profesjonalisty, trafiłeś idealnie! ", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"), new Guid("e07bc534-3324-4af4-8d97-faee7242e896"), "https://open.spotify.com/playlist/37i9dQZF1DWXRqgorJj26U?si=4567abcd1234", 480.0, "Basowe miksy :)" },
                    { new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("ef11919d-3e86-4c08-a594-03800f613fd8"), "Hej, specjalizuje się w produkcji instrumentali, mam w tym duże doświadczenie, bo jestem w branży już od 15 lat. Możliwe, że mnie znasz chociażby z produkcji dla Pezeta, czy Żabsona. Lubie pracować nad trapowymi produkcjami, ale nie ograniczam się tylko do nich - potrafię również robić bity pop'owe. Jeśli szukasz czegoś ciekawego, albo chciałbyś, żebym wykonał dla Ciebie produkcję inspirowaną innym utworem, wystarczy, że do mnie napiszesz, a ja poprowadzę Cię za rękę!", new Guid("b8785564-e008-4889-b633-7f5d3558eb92"), new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"), new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"), "https://open.spotify.com/playlist/2UZk7JjJnbTut1w8fqs3JL", 900.0, "Trapowe bity" },
                    { new Guid("7bfd7cfa-5fde-42e2-ac56-9ee1040b708f"), new Guid("2de15e61-0ab9-49eb-b5e2-cf909809d22f"), "Specjalizuje się w masteringu utworów, głównie dla wytwórni, ale wykonuje również indywidualne zlecenia. Pracuje hybrydowo - ze sprzętem analogowym, ale też z cyfrowymi emulacjami pluginów, tak żeby każdy utwór mógł brzmieć możliwie najlepiej. Masteringiem zajmuje się od 10 lat i ciągle staram się udoskonalać swoje umiejętności. Pracowałam ze Szpakiem, Matą i innymi topowymi artystami z Polskiego podwórka! Możesz być pewien, że utwór z pod mojej ręki, będzie brzmiał najlepiej.", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"), new Guid("731c7617-9342-415d-8e06-f77ec2d56786"), "https://open.spotify.com/playlist/37i9dQZF1DWTcaP2wCKa4K?si=abcdef123456", 300.0, "Najlepszy mastering w Polsce" },
                    { new Guid("8370e2eb-2ea0-4c4e-99e5-b9e719427f03"), new Guid("cedfe8a0-0a9f-4c4a-a50f-76f9fcac396f"), "Cześć, jestem Tomek, jestem producentem muzycznym, który cały swój czas poświęca na odkrywanie nowych i niebanalnych brzmień. Produkcją zajmuje się od 10 lat, więc mam w tym już duży staż. Mogę dla Ciebie wyprodukować instrumentale hip-hop'owe i trap'owe. Korzystam tylko z najlepszych syntezatorów (KORG M1, Yamaha Montage M). Posiadam również szeroką gamę prawdziwych instrumentów (Banjo, Gitary akustyczne, Flety), które moge dodać do produkcji stworzonej pod twoje indywidualne potrzeby. Najlepiej pracuje mi się z pojedynczymi artystami, lecz mogę również podjąć się pracy z zespołem!", new Guid("b8785564-e008-4889-b633-7f5d3558eb92"), new Guid("efe85186-52c9-4c46-b585-d4b47523db47"), new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"), "https://open.spotify.com/playlist/4nZo2X8iHrwhYBYdKvysgI", 800.0, "Unikatowe produkcje i instrumentale" },
                    { new Guid("8cb96d43-a8a9-4010-8613-f721ecedb8b3"), new Guid("0c318716-7c49-4735-9ce2-9eb499377e8a"), "Hej, jestem Michał, jestem inżynierem dźwięku, realizatorem nagrań i reżyserem dźwięku w TVP. W wolnym czasie miksuje utwory dla klientów - robię to z pasji, a nie dla pieniędzy, bo kocham to robić! Pracuje nad różnymi utworami, ale moja specjalizacja to Rock. Jestem biegły w pracy na Reaperze, ale znam również Pro Tools'a i FL Studio. Jeśli wybierzesz mnie, możesz być pewien, że twój miks będzie brzmiał dobrze niezależnie od miejsca - w samochodzie, na słuchawkach, czy w klubie!", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"), new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"), "https://open.spotify.com/playlist/3eoncc59w7c8t1PnKtSOh6", 450.0, "Miksy z pazurem" },
                    { new Guid("a79c87d0-276b-48bd-b23c-9af67afd4c41"), new Guid("c2363242-295c-4435-867c-90d9b96b085a"), "Cześć, jestem Paweł, masteruje piosenki już 5 lat. Wiem, że to niedużo, ale zaufaj mi, że dobrze wiem co robię. Troche o mnie - jestem absolwentem Akademii Realizacji Dźwięku w Warszawie (2020 rok ukończenia) i doskonale wiem, co to znaczy profesjonalny master. Przez długi czas pracowałem jak estradowiec, głównie na koncertach rock'owych, dlatego też, wiem jak ważne jest dobry mastering utworu - jeśli jest słaby, to utwór będzie brzmiał kiepsko na koncercie. Serdecznie zapraszam Cię do współpracy, razem zrobimy coś świetnego!", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"), new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"), "https://open.spotify.com/playlist/37i9dQZF1DWY4xHQp97fN6?si=bcdef7890123", 350.0, "Mastering na poziomie!" },
                    { new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("17cf17e7-cf1d-4239-ba5a-5f8484191038"), "Miksem zajmuje się na codzień, pracując przy utwórach nagrywanych w Filharmonii Warszawskiej. Uczyłem się na UMCS w Warszawie i skończyłem kierunek związany z reżyserią dźwięku. Nie ograniczam się do jednego gatunku, mogę pracować nad utworami pop-owymi, jak i muzyką klasyczną, a nawet hip-hop'em. Pracowałem z artystami takimi jak: Piotr Rogucki, Organek, czy Mietek Szcześniak, dzięki nim zyskałem bardzo dużo doświadczenia w muzyce akustycznej, którą obecnie zajmuje się najwięcej.", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"), new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"), "https://open.spotify.com/playlist/37i9dQZF1DX0XUsuxWHRQd?si=123456abcdef", 400.0, "Profesjonalne miksy z pod ucha specjalisty" }
                });

            migrationBuilder.InsertData(
                table: "SocialMediaLink",
                columns: new[] { "IdSocialMediaLink", "IdSocialMediaName", "IdUser", "Url" },
                values: new object[,]
                {
                    { new Guid("7667e3a7-e8f9-4049-af10-a0a405dacf40"), new Guid("371dbd6d-76eb-4266-aaa3-2b431c5cbafe"), new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "https://www.facebook.com/prod.mustangg/" },
                    { new Guid("8c0d3528-e2cb-430a-bfd4-8e0623c714cf"), new Guid("02c8722f-dccc-4060-bec3-c95815c67703"), new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "https://www.instagram.com/prod.mustang/" },
                    { new Guid("b5b570dd-43c8-471e-976b-91a0d50de9f5"), new Guid("4639c978-26fc-4027-b036-3fc5c0d1d221"), new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "https://www.linkedin.com/in/stanisław-stepka/" }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "IdReview", "Content", "IdAdvert", "IdReviewLog", "IdUser", "SatisfactionLevel" },
                values: new object[,]
                {
                    { new Guid("2d7fe610-9fb0-4a8e-923c-2f7a8afe2a78"), "Polecam każdemu, kto chce mieć master na szybko, w przystępnej cenie i dobrej jakości.", new Guid("7bfd7cfa-5fde-42e2-ac56-9ee1040b708f"), new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"), new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), (byte)4 },
                    { new Guid("5f3bcc4b-d484-44cc-baa2-339373b7d0f0"), "Piękna produkcja! Naprawdę jestem w szoku, że tak szybko dostałem już w pełni gotowego bita, mega!", new Guid("8370e2eb-2ea0-4c4e-99e5-b9e719427f03"), new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"), new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), (byte)5 },
                    { new Guid("649406c7-a59a-4102-b7cb-c39d16bc7117"), "Praca z Piotrem to czysta przyjemność, poinstruował mnie jak wysyłać mu pliki, a sam miks wysłał mi bardzo szybko.", new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"), new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), (byte)5 },
                    { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"), "Miks był okej, ale miałem nadzieje, że Ania bardziej przyłoży się do przeczyszczenia ścieżek, na których było dużo pogłosu!", new Guid("31ba89aa-f10f-40e7-b4b0-7375da567997"), new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"), new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), (byte)3 },
                    { new Guid("dbe3112c-8914-44b1-8011-d58cb2ba4270"), "Krzysiek dobrze wie co robi, zna się na rzeczy, dlatego też moja ocena to 5! Już pierwsza wersją miksu mi się podobała :)", new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"), new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), (byte)5 },
                    { new Guid("f88cb211-6464-4b28-aa48-75f257624d86"), "Super miks, Krzychu zna się na rzeczy, w mojej opinii nie ma nikogo lepszego od niego w branży!", new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"), new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), (byte)5 },
                    { new Guid("fbd98612-7ccd-4b83-afaa-7084e758e746"), "Ehh, strasznie arogancki gość, jeśli chcesz stracić czas to trafiłeś super, bez komentarza...", new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"), new Guid("655887cb-b3cd-40da-b2bb-48b5e84239f9"), (byte)1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advert_IdAdvertCategory",
                table: "Advert",
                column: "IdAdvertCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Advert_IdAdvertLog",
                table: "Advert",
                column: "IdAdvertLog");

            migrationBuilder.CreateIndex(
                name: "IX_Advert_IdUser",
                table: "Advert",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Advert_Title_Description",
                table: "Advert",
                columns: new[] { "Title", "Description" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "IX_HubConnection_IdUser",
                table: "HubConnection",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Review_IdAdvert",
                table: "Review",
                column: "IdAdvert");

            migrationBuilder.CreateIndex(
                name: "IX_Review_IdReviewLog",
                table: "Review",
                column: "IdReviewLog");

            migrationBuilder.CreateIndex(
                name: "IX_Review_IdUser",
                table: "Review",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLink_IdSocialMediaName",
                table: "SocialMediaLink",
                column: "IdSocialMediaName");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLink_IdUser",
                table: "SocialMediaLink",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Token_IdUser",
                table: "Token",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_User_ForCheckAuth",
                table: "User",
                column: "IdUser")
                .Annotation("Npgsql:IndexInclude", new[] { "Email", "FirstName", "LastName", "PhoneNumber", "IdRole" });

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRole",
                table: "User",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUserAuthLog",
                table: "User",
                column: "IdUserAuthLog");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_IdMessage",
                table: "UserMessage",
                column: "IdMessage");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_IdUserRecipient",
                table: "UserMessage",
                column: "IdUserRecipient");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_IdUserSender",
                table: "UserMessage",
                column: "IdUserSender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubConnection");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "SocialMediaLink");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "UserMessage");

            migrationBuilder.DropTable(
                name: "Advert");

            migrationBuilder.DropTable(
                name: "ReviewLog");

            migrationBuilder.DropTable(
                name: "SocialMediaName");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "AdvertCategory");

            migrationBuilder.DropTable(
                name: "AdvertLog");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserAuthLog");
        }
    }
}
