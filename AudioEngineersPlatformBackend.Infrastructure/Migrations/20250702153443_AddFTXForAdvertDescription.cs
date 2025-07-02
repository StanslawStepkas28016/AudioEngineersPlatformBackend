using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFTXForAdvertDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF  EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Advert]'))
                    ALTER FULLTEXT INDEX ON [dbo].[Advert] DISABLE

                GO
                IF  EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Advert]'))
                BEGIN
	                DROP FULLTEXT INDEX ON [dbo].[Advert]
                End

                Go
                IF EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE [name]='AdvertFTC')
                BEGIN
	                DROP FULLTEXT CATALOG AdvertFTC 
                END

                CREATE FULLTEXT CATALOG AdvertFTC AS DEFAULT;
                CREATE FULLTEXT INDEX ON dbo.Advert(Description) KEY INDEX PK_Advert ON AdvertFTC WITH STOPLIST = OFF, CHANGE_TRACKING AUTO;
            ", true);
            
            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 991, DateTimeKind.Utc).AddTicks(5270));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(130));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(130));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(110));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 992, DateTimeKind.Utc).AddTicks(130));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBWmyU0GvFepiYeYW+Jt42xbxWxa2cXXmAleeCfClUdvzwEpymk5/d67s7qIt4s+Sw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEOrXr7gZQTiRpKRPy4t8vPrfRYnSavRgERj/5Cvb3y3QhpUVssVcUzSoT92PI8BvKw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMmoiokAC+ikD2FR3rQCMgFi6WdkbedfpFC8FlhZDRGyruK8DjRIsFTTU/e/Wn4jFg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEIQ5nMAgrTqOvfK1Zd0aV6x8aLzMywCMAighxSbhcD4Rv8TmkLLw4BL2fg6pB+O/xg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEL6uzzm4wpoY2bZcmClP2n5CWmlbZX/9DWwPhMpl1R+g0nAr/lkzweIoQkTUFTmoPA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEqxhUiOo8abEK5N0C6opdp+VRNWspSVopGY8rpvdVPH9Cr7vjFWy3uXLyhXHi3erg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEPikCaFqccCof3nL96/YO+SUSzBEWPu4uqrgs1d2iH322ajL1Ufvpg1DiTY81hixIQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEN769tk6BnP9LWp+9yxUBm6oqivIjTFtWEhHiREWw7s2x7Eisj31TfVHJp66l/WVvQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEKm5aVhWAaklNoNIAvB8Im/Ueq4QxABByM+4hqBi+q6yBVk2HPJCLMA7OFotig2NIQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMwJNZ8YiodASH8rulmLb9yy3YKgZBC6ErZ77dV1vCWeR/kZJ0lrqDZ3BOKT66VQVA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENzFsvaq5ObjvMm1AWw6UW6fxbUPYANf5ZnSjNTGqqLmk8DTM+TkAvP5in8SRHR7SA==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 814, DateTimeKind.Utc).AddTicks(2700));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 601, DateTimeKind.Utc).AddTicks(9090));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 921, DateTimeKind.Utc).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 743, DateTimeKind.Utc).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 529, DateTimeKind.Utc).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 637, DateTimeKind.Utc).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 885, DateTimeKind.Utc).AddTicks(8330));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 708, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 850, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 2, 15, 34, 42, 956, DateTimeKind.Utc).AddTicks(1500), "680085", new DateTime(2025, 7, 3, 15, 34, 42, 956, DateTimeKind.Utc).AddTicks(1510) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 778, DateTimeKind.Utc).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 15, 34, 42, 673, DateTimeKind.Utc).AddTicks(1180));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FULLTEXT INDEX on dbo.Advert;
                DROP FULLTEXT CATALOG AdvertFTC;
            ", true);
            
            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(1550));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6910));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6900));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6830));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6900));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 364, DateTimeKind.Utc).AddTicks(6890));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEPI9Ea64RmWTHzGgU6EbZpUvWV71hRz7K+aiStnYvSP8vVsrhjLC/MVy8zYtmhj5XQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENUPSwPSvjTyygq6/zKq7UonWcMBRG+fOc81yoay+KF7Xbf1OPHsKUg45gGBlFAAkg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEONOjd1XatXtPdXqyNsDBt+ezdMeXjnTzebylO3hvyHZDDOnBmro+yAxieoeiYMWZg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEPVDhtdMCLmR1IwONwg6sJe6p3MjzpoYJhUAEpZNwwKHpgEljS/V4/crH+mvarShKw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEI6HhAyHbv++C1eZiopSxu/VlN6LkfeNdNOtUo6sgeAlMZpKITT/pvrm1iokwNWYMQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENZhOXEWpZJcjgu9M+8cFjxnp9k8fCfPxEDLvd5pRgy+Msa+7NeMLnHqv45fw+b5AQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENUNJcxwJUw7VipksRBAQa9dsDBQN2O8aFNUeDaSUjhfmOwzE9Q2KYHp3J+YgpmZ2Q==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEOmZKkgnpg/1hQvoMpXmT+iOt9cAeLubGwwitpqnCjKCtpC1/ScFPUQdQvK2noW22g==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEKzxhEwbvTapkceMNlDJHBcLt8PqamlgDeQfc3sItVXXleZQNjjdEO6YZVq6anO60g==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEY0VpwQ2ylfAGJDmdqIN/oKn2d92DTAukwsChscp5jrqgP6VzDnqNPHZNCNt7u9rw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDRxjLtZ1uwJTrU64mBSHtV9H/7xVEB13BYvDfSUvXN4a2fid/eX5QycQrWLtpgHEQ==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 183, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 15, 967, DateTimeKind.Utc).AddTicks(8010));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 291, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 111, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 15, 891, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 3, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 255, DateTimeKind.Utc).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 75, DateTimeKind.Utc).AddTicks(650));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 219, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 2, 14, 50, 16, 327, DateTimeKind.Utc).AddTicks(9150), "410942", new DateTime(2025, 7, 3, 14, 50, 16, 327, DateTimeKind.Utc).AddTicks(9210) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 147, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 2, 14, 50, 16, 39, DateTimeKind.Utc).AddTicks(3130));
        }
    }
}
