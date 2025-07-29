using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLinkFromUserToAdvertForSocialMediaLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLink_User",
                table: "SocialMediaLink");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "SocialMediaLink",
                newName: "IdAdvert");

            migrationBuilder.RenameIndex(
                name: "IX_SocialMediaLink_IdUser",
                table: "SocialMediaLink",
                newName: "IX_SocialMediaLink_IdAdvert");

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6340));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6360));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6320));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6330));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6330));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 36, 14, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("7667e3a7-e8f9-4049-af10-a0a405dacf40"),
                column: "IdAdvert",
                value: new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("8c0d3528-e2cb-430a-bfd4-8e0623c714cf"),
                column: "IdAdvert",
                value: new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("b5b570dd-43c8-471e-976b-91a0d50de9f5"),
                column: "IdAdvert",
                value: new Guid("72ac8a29-19e2-4b7b-b810-418d638b5356"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEIMg2MZGmmLch2xSgP14TKMmdi/izjyGwWn5y+kG2wTkvTOtZyEpU0P4CfVFMfiQiA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("156765b0-84a0-4389-af75-78f2f36dea04"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAELR1leH5iV3WYT0G/cCM9aZnaYs9i70h5LFErEEM4eFPflEjxoeK4FUCOov61PQZMA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEIdf5HFE8lz52q4Itsz+NFst0L3lYyg5HlAxszOuvrJDr3hr0z2HTvwhgCDy6H+Oug==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAELjkePwzRTVXiPXLubbGBuq13Y+IaJTGCwTG5datehlIk/dpdaTzELL8rMToBvkMyw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEE8X3mnzjo1aOosd590IBQQz4OT0JKcNQFwB+kwY4Egpx+Gmz0JtzbNsmhv1NfIECQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEHQPUV+FJl59iehnbjbLdXUi95pqdYr/WN5Gi5Dk0RJ62STfGANTK1gmjO1FScavQg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAED24Wfxr2MkCRan8lJ1PmCzkm8VUMjDU7TaeY+VFp4Er1cgAKgA8dNA4Fbk1f3Aeww==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEGBK9m89CwKxN0iBqoisGcidTNp5BV3vLxL9uejO6KTJ79xg8yWR1TLPX+72yc5Q0w==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAELVMNGACEVg3gcofp/cbKjJcirazXxhmrFXFwHkSVc/kd2hdGY1Un5NB3gGhFokUJw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEC7EKB10RzGQhxM5WiyEpilu79oImxoW6r2S4JFqzOucDRAJ6d4KhJ/lfgIR4+BGRg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEuxv6xbsV5lT/jmNMjxGqZXIVIVIMLejQ1P4EJceSoLKfDO2zR4SPSW84BU7KFmLQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENJ4oavSXRRVMj8N194djXEVymSjwviJaXcWykL2nkmdLtkZPbs3K6ZM39V48KtRIg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMHeravCA/XqGT7Z5FC7gw3dk8ca3/ZMT2QcAjnclBHf1OLwsptkiKe0s3KkHHaSog==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 789, DateTimeKind.Utc).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 596, DateTimeKind.Utc).AddTicks(6330));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 887, DateTimeKind.Utc).AddTicks(4460));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 724, DateTimeKind.Utc).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 559, DateTimeKind.Utc).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 629, DateTimeKind.Utc).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 854, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 692, DateTimeKind.Utc).AddTicks(6640));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 949, DateTimeKind.Utc).AddTicks(630));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 822, DateTimeKind.Utc).AddTicks(1510));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 24, 14, 33, 35, 918, DateTimeKind.Utc).AddTicks(2860), "801439", new DateTime(2025, 7, 25, 14, 33, 35, 918, DateTimeKind.Utc).AddTicks(2860) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 981, DateTimeKind.Utc).AddTicks(1880));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 757, DateTimeKind.Utc).AddTicks(80));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 14, 33, 35, 661, DateTimeKind.Utc).AddTicks(8300));

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLink_Advert",
                table: "SocialMediaLink",
                column: "IdAdvert",
                principalTable: "Advert",
                principalColumn: "IdAdvert",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLink_Advert",
                table: "SocialMediaLink");

            migrationBuilder.RenameColumn(
                name: "IdAdvert",
                table: "SocialMediaLink",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_SocialMediaLink_IdAdvert",
                table: "SocialMediaLink",
                newName: "IX_SocialMediaLink_IdUser");

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 875, DateTimeKind.Utc).AddTicks(7460));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2410));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2370));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2410));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2380));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("0d38bc51-218b-4e12-8e39-6bef8654419b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("1f642c35-dbfb-4062-ae75-7cf0e3f27f6f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2980));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("3461a295-b612-4526-aaf1-205ea3a6beff"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("39cc7997-692f-40f4-a3ec-68b00940f6a6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("60bb7ac9-88de-4bd4-933b-ce3e71d9cb45"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("9473bb77-cff3-42d7-bd0e-2807aa2fef52"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "ReviewLog",
                keyColumn: "IdReviewLog",
                keyValue: new Guid("d9de48fd-0abc-4b52-8371-f9f6959fdc46"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 876, DateTimeKind.Utc).AddTicks(2980));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("7667e3a7-e8f9-4049-af10-a0a405dacf40"),
                column: "IdUser",
                value: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("8c0d3528-e2cb-430a-bfd4-8e0623c714cf"),
                column: "IdUser",
                value: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"));

            migrationBuilder.UpdateData(
                table: "SocialMediaLink",
                keyColumn: "IdSocialMediaLink",
                keyValue: new Guid("b5b570dd-43c8-471e-976b-91a0d50de9f5"),
                column: "IdUser",
                value: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEGwOWBaV6pqakHJ18LtzZxsdFFVn3FkitbngvfMGruJT3ufrObMgH9xKYI0lUbUsrQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("156765b0-84a0-4389-af75-78f2f36dea04"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEFuU6tO9CmGW+AAsV+z0ORm53EiO0iRTR9OMNDXL+th6VaUSR/SyFRvPRxQwioiNVw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDHCmqYDIT267Bqyb66OUFsNTpcnmqf5C7D1Die4g1S7crtEF2uf6G1H1UlIuEiBcQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAO4r9i2AWTzU5vZF9YHqgsMerSES9BLkr/jS2MJXmCS+w4I9yId0q2dMBAToVd8fA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAECqnCpSaKUneL1dnXCRYPHs63qsWVeVaRQeSlqq62M2nejH52jVghdShUajvbSEYjA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAVdK5WZsW4zffxbCIRiUS+sHygqax789nPxxO47U8SLuKAv1Ml5DfKU9hQnUFLEQQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDijGo4uRvO0g/FE0v4R5zi1dZwoUxUSgqx+ORr9O7tMY47nPPuJ9IkhfMViveH8zQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBBJb8hkJ/AEdy+KiyrYvXBZ0GT7sJKFElfrZQlRqpjWfcfhL8pCJx2GfTwyoM8ONQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJcB7Wkqkg9PgOaGWKL/qTNo26/3350OPQckVWUfY6n96PRwNV33LEUTlRxpPtNsog==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEFTXmyIglWgMq0dc4RG8glrMP+vNj8ESqF4JZ3dHdCdGbo303kCYtS/VvHH+PSD09g==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEM88pV3sqB5dX33vS2YyHFrnES06+fA2UMqLngG/TkEqQlBjT5HpDbToVYiY0C8iw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAS3Z0MChHU7pkkxfV/hFVbQGa+t+c5230zu2kMFp1Hw2kJGGZW5PgGM+PYGhm/OeQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAELVoYiAa4fJKZrbNgFzPB6ca1SqzCfw8ZmEGOQ/0Vk3O4nUmHdjZLT+hrbfkrPVe9A==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 639, DateTimeKind.Utc).AddTicks(5470));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 434, DateTimeKind.Utc).AddTicks(1570));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 740, DateTimeKind.Utc).AddTicks(1800));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 569, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 396, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 466, DateTimeKind.Utc).AddTicks(7620));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 707, DateTimeKind.Utc).AddTicks(1250));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 532, DateTimeKind.Utc).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 807, DateTimeKind.Utc).AddTicks(2180));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 674, DateTimeKind.Utc).AddTicks(1730));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 24, 9, 46, 49, 773, DateTimeKind.Utc).AddTicks(5930), "620609", new DateTime(2025, 7, 25, 9, 46, 49, 773, DateTimeKind.Utc).AddTicks(5990) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 840, DateTimeKind.Utc).AddTicks(7890));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 604, DateTimeKind.Utc).AddTicks(8340));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 24, 9, 46, 49, 499, DateTimeKind.Utc).AddTicks(2840));

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLink_User",
                table: "SocialMediaLink",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
