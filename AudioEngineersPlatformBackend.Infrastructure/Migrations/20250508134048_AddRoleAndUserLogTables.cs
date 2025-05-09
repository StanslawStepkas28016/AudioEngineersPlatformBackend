using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndUserLogTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "User_Role_FK",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "User_PK",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "Role_PK",
                table: "Role");

            migrationBuilder.AddColumn<Guid>(
                name: "IdUserLog",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "IdUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "IdRole");

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
                    DateLastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLog", x => x.IdUserLog);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUserLog",
                table: "User",
                column: "IdUserLog");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role",
                table: "User",
                column: "IdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserLog",
                table: "User",
                column: "IdUserLog",
                principalTable: "UserLog",
                principalColumn: "IdUserLog",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserLog",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_IdUserLog",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "IdUserLog",
                table: "User");

            migrationBuilder.AddPrimaryKey(
                name: "User_PK",
                table: "User",
                column: "IdUser");

            migrationBuilder.AddPrimaryKey(
                name: "Role_PK",
                table: "Role",
                column: "IdRole");

            migrationBuilder.AddForeignKey(
                name: "User_Role_FK",
                table: "User",
                column: "IdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
