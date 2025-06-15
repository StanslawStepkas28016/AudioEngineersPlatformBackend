using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExp",
                table: "UserLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "IdRole",
                keyValue: new Guid("5c9a3c43-8f4e-4c1e-a5f3-8e3cdbe0158a"),
                column: "RoleName",
                value: "Administrator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExp",
                table: "UserLog");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "IdRole",
                keyValue: new Guid("5c9a3c43-8f4e-4c1e-a5f3-8e3cdbe0158a"),
                column: "RoleName",
                value: "Admin");
        }
    }
}
