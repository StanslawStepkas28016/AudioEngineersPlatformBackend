using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "UserLog",
                columns: table => new
                {
                    IdUserLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationCodeExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    DateLastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    IdAdvertCategory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAdvertLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advert", x => x.IdAdvert);
                    table.ForeignKey(
                        name: "FK_Advert_AdvertCategory",
                        column: x => x.IdAdvertCategory,
                        principalTable: "AdvertCategory",
                        principalColumn: "IdAdvertCategory",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                table: "AdvertCategory",
                columns: new[] { "IdAdvertCategory", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("13e214e4-7eca-4237-8d67-295590d348fa"), "Mixing" },
                    { new Guid("c2d9e584-1380-4826-ab92-0061fe88074c"), "Production" },
                    { new Guid("cb05eb29-16ee-44a0-b35b-c8c33f326589"), "Mastering" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "IdRole", "RoleName" },
                values: new object[,]
                {
                    { new Guid("2517c538-a4e6-48ac-86f4-948b068fa197"), "Audio engineer" },
                    { new Guid("4b80dfb0-d3c3-4188-a67e-32f3c3d88635"), "Client" },
                    { new Guid("6564407b-cb51-42f2-b399-a1c25328f156"), "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "UserLog",
                columns: new[] { "IdUserLog", "DateCreated", "DateDeleted", "DateLastLogin", "IsDeleted", "IsVerified", "RefreshToken", "RefreshTokenExp", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[,]
                {
                    { new Guid("8e00b20f-059b-447d-a203-89d984e4f300"), new DateTime(2025, 6, 18, 14, 17, 10, 351, DateTimeKind.Utc).AddTicks(4010), null, null, false, true, null, null, null, null },
                    { new Guid("d75cdc18-793b-4aba-8e7f-87c4362469b4"), new DateTime(2025, 6, 18, 14, 17, 10, 247, DateTimeKind.Utc).AddTicks(7510), null, null, false, true, null, null, null, null },
                    { new Guid("dc9862b8-b7dc-4062-8e2c-14c972e9205b"), new DateTime(2025, 6, 18, 14, 17, 10, 317, DateTimeKind.Utc).AddTicks(8320), null, null, false, true, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "IdUser", "Email", "FirstName", "IdRole", "IdUserLog", "LastName", "Password", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("3c0f422d-0d47-40cb-8585-5ab6e776928a"), "jan.nowak@gmail.com", "Jan", new Guid("4b80dfb0-d3c3-4188-a67e-32f3c3d88635"), new Guid("dc9862b8-b7dc-4062-8e2c-14c972e9205b"), "Nowak", "AQAAAAIAAYagAAAAEHcCWmnz5Op3yVRtGZCT4tMaNStTk17PaLwkWWWQ1c62v0S/Cq+Fmfbu5Cu9kfrC5w==", "+48696432123" },
                    { new Guid("3f4fd287-be8c-4b9c-a47d-de0d2311de8d"), "dominik.kow@gmail.com", "Dominik", new Guid("6564407b-cb51-42f2-b399-a1c25328f156"), new Guid("d75cdc18-793b-4aba-8e7f-87c4362469b4"), "Kowalski", "AQAAAAIAAYagAAAAEHwqXHYDVB5J6I27jS861k8QSpE7pfUpset7kQsfyGpZqlFBd5VF1RJHU/uK/pbQPA==", "+48123456789" },
                    { new Guid("efe2d4fc-27b0-40e9-a3c2-f9721e71356c"), "anna.kow@gmail.com", "Anna", new Guid("2517c538-a4e6-48ac-86f4-948b068fa197"), new Guid("8e00b20f-059b-447d-a203-89d984e4f300"), "Kowalska", "AQAAAAIAAYagAAAAENzHsf7H/lBxfSUz0TjzrwCFOchEXKE/T2pfbiV396DprKf9L6VYihtdm7KavMEz7w==", "+48543123123" }
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
                name: "Advert");

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
