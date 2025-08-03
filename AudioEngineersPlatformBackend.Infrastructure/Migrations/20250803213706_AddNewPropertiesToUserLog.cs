using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesToUserLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertCategory",
                columns: table => new
                {
                    IdAdvertCategory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertCategory", x => x.IdAdvertCategory);
                });

            migrationBuilder.CreateTable(
                name: "AdvertLog",
                columns: table => new
                {
                    IdAdvertLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertLog", x => x.IdAdvertLog);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLog",
                columns: table => new
                {
                    IdReviewLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLog", x => x.IdReviewLog);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaName",
                columns: table => new
                {
                    IdSocialMediaName = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaName", x => x.IdSocialMediaName);
                });

            migrationBuilder.CreateTable(
                name: "UserLog",
                columns: table => new
                {
                    IdUserLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationCodeExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    ResetEmailToken = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResetEmailTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResettingEmail = table.Column<bool>(type: "bit", nullable: false),
                    ResetPasswordToken = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResetPasswordTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResettingPassword = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLog", x => x.IdUserLog);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUserLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        column: x => x.IdUserLog,
                        principalTable: "UserLog",
                        principalColumn: "IdUserLog",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Advert",
                columns: table => new
                {
                    IdAdvert = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAdvertCategory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAdvertLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "SocialMediaLink",
                columns: table => new
                {
                    IdSocialMediaLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdSocialMediaName = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "Review",
                columns: table => new
                {
                    IdReview = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SatisfactionLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    IdAdvert = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdReviewLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    { new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(3030), null, null, true, false },
                    { new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7850), null, null, true, false },
                    { new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7870), null, null, true, false },
                    { new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7860), null, null, true, false },
                    { new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7800), null, null, true, false },
                    { new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7840), null, null, true, false },
                    { new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7870), null, null, true, false },
                    { new Guid("efe85186-52c9-4c46-b585-d4b47523db47"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7850), null, null, true, false },
                    { new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(7860), null, null, true, false }
                });

            migrationBuilder.InsertData(
                table: "ReviewLog",
                columns: new[] { "IdReviewLog", "DateCreated", "DateDeleted", "IsDeleted" },
                values: new object[,]
                {
                    { new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false },
                    { new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"), new DateTime(2025, 8, 3, 21, 37, 6, 393, DateTimeKind.Utc).AddTicks(8450), null, false }
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
                table: "UserLog",
                columns: new[] { "IdUserLog", "DateCreated", "DateDeleted", "DateLastLogin", "IsDeleted", "IsResettingEmail", "IsResettingPassword", "IsVerified", "RefreshToken", "RefreshTokenExpiration", "ResetEmailToken", "ResetEmailTokenExpiration", "ResetPasswordToken", "ResetPasswordTokenExpiration", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[,]
                {
                    { new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"), new DateTime(2025, 8, 3, 21, 37, 6, 151, DateTimeKind.Utc).AddTicks(4820), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("2f765163-6728-48bc-9767-66687efdf86e"), new DateTime(2025, 8, 3, 21, 37, 5, 943, DateTimeKind.Utc).AddTicks(5300), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"), new DateTime(2025, 8, 3, 21, 37, 6, 257, DateTimeKind.Utc).AddTicks(810), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"), new DateTime(2025, 8, 3, 21, 37, 6, 83, DateTimeKind.Utc).AddTicks(170), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"), new DateTime(2025, 8, 3, 21, 37, 5, 903, DateTimeKind.Utc).AddTicks(1600), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"), new DateTime(2025, 8, 3, 21, 37, 5, 978, DateTimeKind.Utc).AddTicks(9340), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"), new DateTime(2025, 8, 3, 21, 37, 6, 221, DateTimeKind.Utc).AddTicks(5750), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"), new DateTime(2025, 8, 3, 21, 37, 6, 48, DateTimeKind.Utc).AddTicks(3150), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"), new DateTime(2025, 8, 3, 21, 37, 6, 326, DateTimeKind.Utc).AddTicks(670), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"), new DateTime(2025, 8, 3, 21, 37, 6, 186, DateTimeKind.Utc).AddTicks(2000), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"), new DateTime(2025, 8, 3, 21, 37, 6, 292, DateTimeKind.Utc).AddTicks(1290), null, null, false, false, false, false, null, null, null, null, null, null, "554903", new DateTime(2025, 8, 4, 21, 37, 6, 292, DateTimeKind.Utc).AddTicks(1350) },
                    { new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"), new DateTime(2025, 8, 3, 21, 37, 6, 359, DateTimeKind.Utc).AddTicks(8280), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("df0a8813-0938-42a6-ac84-26298701f456"), new DateTime(2025, 8, 3, 21, 37, 6, 117, DateTimeKind.Utc).AddTicks(860), null, null, false, false, false, true, null, null, null, null, null, null, null, null },
                    { new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"), new DateTime(2025, 8, 3, 21, 37, 6, 14, DateTimeKind.Utc).AddTicks(1490), null, null, false, false, false, true, null, null, null, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "IdUser", "Email", "FirstName", "IdRole", "IdUserLog", "LastName", "Password", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"), "krzysztof.lewandowski@example.com", "Krzysztof", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"), "Lewandowski", "AQAAAAIAAYagAAAAEHpahjVOx0nhDLWUOHzAAf2ABpjD7gFgra4nnpLIAa39kpkozbJgN6C++UI1baxALQ==", "+48111777888" },
                    { new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), "dab@gmail.pl", "Maria", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"), "Dąbrowska", "AQAAAAIAAYagAAAAEItYbSAWYJs2hZ9P/M2u68D3qRYIN1EZdICz+31iVERpNdC7Im8V0IZq8sIqZz2KJg==", "+48231443225" },
                    { new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"), "pawel.kaminski@example.com", "Paweł", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"), "Kamiński", "AQAAAAIAAYagAAAAEI15m5rX9vaKPsm3S3aysBZbqrTdNKdnExvbeh8nLxLsJfRh5QGnRzIxllbp+ov09w==", "+48111999000" },
                    { new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"), "piotr.nowak@example.com", "Piotr", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"), "Nowak", "AQAAAAIAAYagAAAAEDYN0PVfEgLl5LX216l1A8sT+WNZRPb/7X5e6+J1oIynq+HvFXDX2khiHdtROuEEtg==", "+48111222333" },
                    { new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"), "tomasz.zielinski@example.com", "Tomasz", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"), "Zieliński", "AQAAAAIAAYagAAAAEIWDE5TtLIOP1QdeQiBe9u1zKZkj/7F5opBDWEuS9VJRxX/UodnQ+BPtQbZYDPfhMw==", "+48111444555" },
                    { new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"), "michal.wojcik@example.com", "Michał", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("df0a8813-0938-42a6-ac84-26298701f456"), "Wójcik", "AQAAAAIAAYagAAAAEEZgI/OOGv7KN0YHUglKOILSBaSQ29RzVdkRoJHBfOaDhL+443V52iJSvTT9jj1EMg==", "+48111555666" },
                    { new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), "jan.nowak@gmail.com", "Jan", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("2f765163-6728-48bc-9767-66687efdf86e"), "Nowak", "AQAAAAIAAYagAAAAEDY2G6jb2oqEW3nUk3UIMGHEqeDYCdQALRCYXeJLfs0Q164GMghfaWtkZqf6vFD9Hg==", "+48696432123" },
                    { new Guid("655887cb-b3cd-40da-b2bb-48b5e84239f9"), "mar.radw@example.com", "Marcin", new Guid("d92d29b8-f462-46df-8efb-de6b9aa5109a"), new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"), "Radwański", "test", "+48431234765" },
                    { new Guid("731c7617-9342-415d-8e06-f77ec2d56786"), "ewa.maj@example.com", "Ewa", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"), "Maj", "AQAAAAIAAYagAAAAELzoDl53H8ShqzHbrpZY5YOy/rkcjVNrSJsdMN1MEJirjCoAEWoW3nyagyIvefoK0Q==", "+48111333444" },
                    { new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "anna.kow@gmail.com", "Anna", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"), "Kowalska", "AQAAAAIAAYagAAAAEB2wih5/4wnlHNcGv7meVlyX7jN/lDpCy7j752zBYz3qhwpQxJZuVL9hbICdVzm1Pw==", "+48543123123" },
                    { new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"), "katarzyna.wisniewska@example.com", "Katarzyna", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"), "Wiśniewska", "AQAAAAIAAYagAAAAEJSjmR0UV6cU6mHCdU57TRE+rv9EgGKf5TfgwbpvXKbLmASCBYCaL6E/Ig1Jvwctxw==", "+48111666777" },
                    { new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"), "dominik.kow@gmail.com", "Dominik", new Guid("d92d29b8-f462-46df-8efb-de6b9aa5109a"), new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"), "Kowalski", "AQAAAAIAAYagAAAAECrAMg55LZuQYIQUWibIMp1NZFZWyGMQwr5ZDt92qo2lQGQEbKG/rCy0xXpseFTf4g==", "+48123456789" },
                    { new Guid("e07bc534-3324-4af4-8d97-faee7242e896"), "agnieszka.wrobel@example.com", "Agnieszka", new Guid("522c6700-165e-4189-b234-9fb533266e07"), new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"), "Wróbel", "AQAAAAIAAYagAAAAED35Eslk8gj+6cD49WZ1LuD36uu3ffnJagxFbGFtGvDSODxJDTD5CPECV4df2QIc4Q==", "+48111888999" },
                    { new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), "marian@gmail.pl", "Marian", new Guid("004865e2-177f-4c54-bb4c-69799f0bf315"), new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"), "Niewiadomski", "AQAAAAIAAYagAAAAEIx90/FDEGbe1HYPj+LHee1psM3PTvGKSVvq34u8oBfEFI8w7ARuzTIgNTYu98Cfqg==", "+48654123432" }
                });

            migrationBuilder.InsertData(
                table: "Advert",
                columns: new[] { "IdAdvert", "CoverImageKey", "Description", "IdAdvertCategory", "IdAdvertLog", "IdUser", "PortfolioUrl", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("18809be2-b063-4ada-a7a4-81f9fa107322"), new Guid("504cd9ce-5804-4f76-b6bf-706aae87a1b0"), "Katarzyna offers specialized mastering for both vinyl pressings and digital platforms. She carefully sequences tracks, applies EQ to prevent low-end overmodulation, and optimizes side-chain compression for needle-friendly dynamics. For streaming masters, she fine-tunes loudness to meet platform standards (Spotify, Apple Music, Tidal) while preserving headroom and musicality. You’ll get final masters in DDP, WAV, and MP3 formats, plus an analytical Loudness Unit Full Scale (LUFS) report. Elevate your project with a mastering engineer who understands the nuances of different playback mediums.", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"), new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"), "https://open.spotify.com/playlist/37i9dQZF1DX4WYpdgoIcn6?si=abcd3456ef90", 320.0, "Mastering for vinyl & streaming—Katarzyna" },
                    { new Guid("31ba89aa-f10f-40e7-b4b0-7375da567997"), new Guid("df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e"), "With over 10 years of hands-on experience in music mixing, I meticulously balance every element of your track—from drums and bass to vocals and effects—to ensure a polished, radio-ready sound. I use industry-standard tools and reference mixes to match the tonal character and loudness of top-charting songs. Whether you need depth, clarity, or that modern “in-your-face” sheen, I’ll tailor my approach to your genre and artistic vision. Turn your rough stems into a cohesive, dynamic mix that translates across all playback systems.", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"), new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"), "https://open.spotify.com/playlist/37i9dQZF1DZ06evO4pPsgW?si=e069a7940cc7419b", 350.0, "I will mix your song professionally!" },
                    { new Guid("33545021-1ffb-4f46-9df8-6242b8f0786f"), new Guid("d98dd161-cd11-4397-b6b2-48a5656c20a3"), "Agnieszka takes a surgical approach to mixing: corrective EQ, transparent compression, and creative spatial effects that serve your song. She communicates clearly, providing time-stamped revision notes and A/B comparisons. Using analog summing and high-end outboard gear, she injects warmth and depth, then returns to the box for final automation rides. You receive both instrumental and vocal stems plus a mastered reference for quick upload. Ideal for artists who demand both technical accuracy and emotional impact in their mixes.", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"), new Guid("e07bc534-3324-4af4-8d97-faee7242e896"), "https://open.spotify.com/playlist/37i9dQZF1DWXRqgorJj26U?si=4567abcd1234", 480.0, "Mix engineering—Agnieszka’s precision approach" },
                    { new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("ef11919d-3e86-4c08-a594-03800f613fd8"), "Krzysztof specializes in crafting genre-blending beats—from trap and lo-fi to funk and soul. Each beat comes with full MIDI programming, drum samples, and multitrack stems so you can rearrange or remix at will. He also offers vocal comping and editing as an add-on, ensuring your performance sits perfectly in the groove. Expect high-quality WAVs, labeled session files, and a quick turnaround. Perfect for rappers, singers, and producers looking for fresh, customizable sound beds.", new Guid("b8785564-e008-4889-b633-7f5d3558eb92"), new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"), new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"), "https://open.spotify.com/playlist/2UZk7JjJnbTut1w8fqs3JL", 900.0, "Beat production & stems by Krzysztof" },
                    { new Guid("7bfd7cfa-5fde-42e2-ac56-9ee1040b708f"), new Guid("2de15e61-0ab9-49eb-b5e2-cf909809d22f"), "Ewa specializes in mastering both digital and analog formats, delivering loudness-optimized masters without sacrificing dynamic range. She uses high-resolution metering and custom multiband compression to sculpt frequencies, tame harshness, and add that final sheen. Your track will be delivered in multiple formats (WAV, MP3, DDP) with ISRC embedding and CD-ready files if needed. Ewa also provides detailed EQ and loudness reports so you know exactly how your music will perform on Spotify, Apple Music, and vinyl pressings. Bring your mixes to the next level with transparent, professional mastering.", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"), new Guid("731c7617-9342-415d-8e06-f77ec2d56786"), "https://open.spotify.com/playlist/37i9dQZF1DWTcaP2wCKa4K?si=abcdef123456", 300.0, "Mastering expertise by Ewa" },
                    { new Guid("8370e2eb-2ea0-4c4e-99e5-b9e719427f03"), new Guid("cedfe8a0-0a9f-4c4a-a50f-76f9fcac396f"), "Tomasz offers end-to-end music production: from songwriting support and beat programming to arrangement and mix-ready stems. He crafts custom drum patterns, bass lines, and melodic hooks tailored to your style. Using both software synths and hardware outboard gear, he delivers a modern, dynamic sound that stands out in today’s crowded market. Each package includes at least three revision rounds, MIDI files for your own tweaks, and guidance on vocals and performance recording. Ideal for solo artists, bands, and labels seeking a cohesive sonic identity.", new Guid("b8785564-e008-4889-b633-7f5d3558eb92"), new Guid("efe85186-52c9-4c46-b585-d4b47523db47"), new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"), "https://open.spotify.com/playlist/4nZo2X8iHrwhYBYdKvysgI", 800.0, "Full production package from Tomasz" },
                    { new Guid("8cb96d43-a8a9-4010-8613-f721ecedb8b3"), new Guid("0c318716-7c49-4735-9ce2-9eb499377e8a"), "Michał combines both in-the-box precision and analog warmth to achieve a balanced, lively mix. He employs gain-riding automation, mid/side processing, and parallel compression to bring out the emotion in your performance. With fluency across Pro Tools, Logic Pro, and Ableton Live, he adapts to your session templates and plugin suites seamlessly. You’ll receive detailed session notes, dry/wet stems, and high-resolution WAV master ready for distribution. Whether it’s a cinematic score or an underground hip-hop track, Michał’s mixes translate beautifully across car stereos, club systems, and earbuds.", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"), new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"), "https://open.spotify.com/playlist/3eoncc59w7c8t1PnKtSOh6", 450.0, "Advanced mixing workflows by Michał" },
                    { new Guid("a79c87d0-276b-48bd-b23c-9af67afd4c41"), new Guid("c2363242-295c-4435-867c-90d9b96b085a"), "Paweł provides a full delivery package: mastered WAV, high-quality MP3, and separated stems for remixers or video post-production. He focuses on dynamic control, spectral balance, and proper headroom for broadcast. He also embeds metadata (ISRC, artist name, album art) so you can deliver directly to digital distributors with confidence. Comprehensive test masters are supplied so you can preview on headphones, car, and club systems. Get radio-ready masters that truly represent your artistic vision.", new Guid("80c20081-c580-4aaf-a346-2587ccfdebf5"), new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"), new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"), "https://open.spotify.com/playlist/37i9dQZF1DWY4xHQp97fN6?si=bcdef7890123", 350.0, "Mastering & delivery by Paweł" },
                    { new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("17cf17e7-cf1d-4239-ba5a-5f8484191038"), "Piotr brings 5+ years of mixing expertise in genres ranging from indie rock to electronic dance. He begins each project by analyzing your reference tracks and customizing EQ, compression, and spatial effects to enhance clarity and impact. His workflow includes detailed vocal tuning, side-chain ducking for punchy rhythms, and analog emulation for warm, musical saturation. Expect thorough revision rounds and clear communication every step of the way. Let Piotr transform your raw sessions into a powerful, polished mix that stands out on streaming platforms and live stages alike.", new Guid("e6ddd487-8b56-4c8f-b289-2f04babbabda"), new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"), new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"), "https://open.spotify.com/playlist/37i9dQZF1DX0XUsuxWHRQd?si=123456abcdef", 400.0, "Professional mixing services by Piotr" }
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
                    { new Guid("2d7fe610-9fb0-4a8e-923c-2f7a8afe2a78"), "I feel like this person knows their craft and is capable of delivering a good mix. I can most definitely recommend working with them :)", new Guid("7bfd7cfa-5fde-42e2-ac56-9ee1040b708f"), new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"), new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), (byte)4 },
                    { new Guid("5f3bcc4b-d484-44cc-baa2-339373b7d0f0"), "Great production skills, I am very happy with the final result!", new Guid("8370e2eb-2ea0-4c4e-99e5-b9e719427f03"), new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"), new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), (byte)5 },
                    { new Guid("649406c7-a59a-4102-b7cb-c39d16bc7117"), "They way I was treated was great. I will most definitely visit this engineer in the studio again!", new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"), new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), (byte)5 },
                    { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"), "The mix was good, but I expected more attention to detail. Overall, a decent experience.", new Guid("31ba89aa-f10f-40e7-b4b0-7375da567997"), new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"), new Guid("156765b0-84a0-4389-af75-78f2f36dea04"), (byte)3 },
                    { new Guid("dbe3112c-8914-44b1-8011-d58cb2ba4270"), "I feel like the engineer could not really achieve what I have wanted, however I think they were really patient and creative ;)", new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"), new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"), (byte)3 },
                    { new Guid("f88cb211-6464-4b28-aa48-75f257624d86"), "Excellent mixes, I have never worked with such a talented engineer in my life. I will recommend working with him all the way!", new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"), new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"), new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"), (byte)5 },
                    { new Guid("fbd98612-7ccd-4b83-afaa-7084e758e746"), "Poor judgement, does not understand the music I like and hates on doing some additional revisions of a master, I can't recommend this person...", new Guid("aff251d8-9e58-4f5c-ba43-4c6597fc8a08"), new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"), new Guid("655887cb-b3cd-40da-b2bb-48b5e84239f9"), (byte)1 }
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
                name: "IX_User_ForCheckAuth",
                table: "User",
                column: "IdUser")
                .Annotation("SqlServer:Include", new[] { "Email", "FirstName", "LastName", "PhoneNumber", "IdRole" });

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRole",
                table: "User",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUserLog",
                table: "User",
                column: "IdUserLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "SocialMediaLink");

            migrationBuilder.DropTable(
                name: "Advert");

            migrationBuilder.DropTable(
                name: "ReviewLog");

            migrationBuilder.DropTable(
                name: "SocialMediaName");

            migrationBuilder.DropTable(
                name: "AdvertCategory");

            migrationBuilder.DropTable(
                name: "AdvertLog");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserLog");
        }
    }
}
