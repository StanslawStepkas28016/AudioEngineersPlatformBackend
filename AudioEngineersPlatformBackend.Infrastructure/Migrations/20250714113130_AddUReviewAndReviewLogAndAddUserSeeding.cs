using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUReviewAndReviewLogAndAddUserSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ReviewLog",
                newName: "DateCreated");

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 187, DateTimeKind.Utc).AddTicks(8030));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3020));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(2970));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3000));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 188, DateTimeKind.Utc).AddTicks(3020));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEHzXzXgWcyF9QFRh3VKdb+0di+SldWfLZL5wmF6kf9+mFYjnYgPocOdLqSwslG+5Tg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("156765b0-84a0-4389-af75-78f2f36dea04"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEOYZoaPhoo1KuOvl010pX6pNchcr+fgBNXWthYjQXclklWyo3IMkbAZN3TtpcvIqwA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJaqKWBt+tGFSA3AJO2cjFsJAOTWallZgmBLcq+T58EMDqbhYTWeA1QmrTzlwm2jog==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEALV5Ctuu55karTken8j2gxHsnZbWeYoSW4PUXI01agXKsKhzUNDG9y/FrzKTQkL1Q==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEACw10rFpDTCSsDhierAD82uhZrEvFY6nY/B+q5Mi7F3xSMDFJNhUprRVnUZCFMtzQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJdptrbd+j+xPX9jiesXUcg+Zdp9iSvsOZz0QBRNXRwFRweMkeVFo4W0K1VrooTn3A==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEu8Bdlta7ntDbw9MeEG2aOuQItwk2Pz9vLtgC/Y2pnjPvkQAc5xwfN5n0zSUxeIqA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEB5ow4pGhm2ZXYMNsybjmOqeV5o9pXLofV6sJ5Wj4LZmsoHQ7uOJWeEbV4NNov+g5A==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAECYLrSoeYfZ1qWihSLNcwWetSGfhYGGqqTPozzRJNdaQJdlzxHmlldJ5Dv8ur3WROw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEH183m9REzf6TQslOYQmYUQvj0mBuwQLpTOHWkMeoDU/hNnxjf8fVvnD9LdzEWF4Ng==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEEhsKgxJS/x5/QSRUObz/u1DP1x6rKdh4uRDLkxqbL7vP+62BpvTkJYiKBV93av/ag==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAECFBTyFIA3qSeyxWrExnJrub7/5Hx9tcMlLCeick9u7nAuShG8iD5lPbvUG/D7PPZw==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMTGLiJOIoFO6qgo/EwYKriSrnQWwbOvSqNl8NcCULTUUBLYBomj1N138EOP4l40Tg==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 947, DateTimeKind.Utc).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 737, DateTimeKind.Utc).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 50, DateTimeKind.Utc).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 877, DateTimeKind.Utc).AddTicks(2320));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 663, DateTimeKind.Utc).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 772, DateTimeKind.Utc).AddTicks(3620));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 17, DateTimeKind.Utc).AddTicks(1630));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 842, DateTimeKind.Utc).AddTicks(6760));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 118, DateTimeKind.Utc).AddTicks(8920));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 982, DateTimeKind.Utc).AddTicks(2970));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 14, 11, 31, 30, 84, DateTimeKind.Utc).AddTicks(2940), "103614", new DateTime(2025, 7, 15, 11, 31, 30, 84, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 30, 153, DateTimeKind.Utc).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 912, DateTimeKind.Utc).AddTicks(4220));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 11, 31, 29, 807, DateTimeKind.Utc).AddTicks(4290));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "ReviewLog",
                newName: "CreatedAt");

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("1b84601e-e225-4e9d-93d2-911fb0a1569e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 192, DateTimeKind.Utc).AddTicks(5950));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("24ba6029-f88c-4b12-9a63-bf00c2d9f3e4"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(910));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("70820368-d390-4c01-af1f-9e7b8e8413d2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("993648ca-9d51-419a-85e8-046e8fc3162b"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(930));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("a9e9762a-2a67-46f3-b371-50405a100d58"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("b3d2dce1-a858-4312-937d-c56a6e0178cf"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(850));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("c8ab7e20-e7dd-4616-8862-d15dad3c986a"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("efe85186-52c9-4c46-b585-d4b47523db47"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(900));

            migrationBuilder.UpdateData(
                table: "AdvertLog",
                keyColumn: "IdAdvertLog",
                keyValue: new Guid("fe0d1832-793e-4cf8-983a-bbe09d7e0fa2"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 193, DateTimeKind.Utc).AddTicks(920));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("07434fd4-3450-4a01-a8c4-c371ed011e48"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEPjJcmyMVnyNPkVa4BXGYDaH0FAA+/Phuwa7WcHtQsmNBb/Mq+KY8L+sIGtPTfxENg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("156765b0-84a0-4389-af75-78f2f36dea04"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEKpHFwqXC6N4GZOKx1mjJwLYV7UuWdHhlxxZr7/wki/3GAEIyDTftv9z0sxqyFh/8A==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("1d31a511-8d38-4223-96a0-f2b15cc90794"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEIrPlrjAcwl3FnymNtru4alTmghdfYdoNMLTWHfn0Dil5yO1RDpRPDTFNOgKc95w9Q==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("2254933a-66ac-4ab8-a923-25d508d8b5c0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBELdK7fJp1uxFEOp5kSBIQ+4bFA7P2VWlXIYNRzIGFbZO7oWAsLFrVsxDyyxiWKrg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("29d1d9bd-87d9-4125-99a5-0f15c9df3a30"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAENLxdh2QaMt+N79PzBuxWnHSY7N77scIxDXQRdTqpaUslY3zISEwTwGTaoXWQz0Hug==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("3fb9e066-38b7-42ae-900c-d7ab5ae280f0"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEIlrsEfK48PLEUVuYVya2q8I5oUmX6l8arLJI2NUyJtSGnci+PN/e0HrBLu2L1l5AQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("5bfc9c8d-4789-4065-99d9-81ec5b58c0f5"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEENxudLNPuJG/XQAuc/P9f6yCSA9SQNwpyaoKlCoFDWN9eg/F1WWdFm7Cr/RwC4MxA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("731c7617-9342-415d-8e06-f77ec2d56786"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEK2hurTK7Z7+mfpdFwUs13s/DmXa/9S260NnpaOVslQ8QDeJAwaXKGUCDyb3Rj39FQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEGEx5thzSt7eS2OvxmWQz0CKsgRNgiKudjbb+pys9TUJrDZrOuzN5lR6+9vW9NlGGg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("ac89f1a4-6988-4211-8136-fbf9b45e4cf2"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEOJvsWYdBkfZPdIruezW8GT+ywzsJwbUnyLr2/BxcHiMY2cH2b5nlytolMwF019ivA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("aebc2724-0edf-4691-99e9-65cbd3aab3bf"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDKctg9Wik2UsVkTxYPsA9RM2v7EbKjnvsocAQ85gPD7xnOzZI92TpkfPJG0CfjA3g==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("e07bc534-3324-4af4-8d97-faee7242e896"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAED1L0Sgm3YskdsT/YX2K4tGEsjkR449yMF8rkRh0raoV9slH51p0SJElMs5ETFVnEg==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "IdUser",
                keyValue: new Guid("fdf7bda4-f40f-484f-bc40-adbf8aa98985"),
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJiyb8ww5ljLaW5jaxI9gZe+MayOiMhjiGomjw74T9DjU5UwSyvZyW+/Zf1XwkbG2A==");

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2a019dc8-fe9f-4a63-b692-49e03f889f7f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 951, DateTimeKind.Utc).AddTicks(4670));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("2f765163-6728-48bc-9767-66687efdf86e"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 743, DateTimeKind.Utc).AddTicks(6410));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("32affe63-9bb3-4c86-bbf8-6d5e37c7fb3f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 54, DateTimeKind.Utc).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5091bf83-df7d-4a54-a35b-31b44d1a1643"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 882, DateTimeKind.Utc).AddTicks(2160));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("5cb8efaa-2432-46d1-9984-b41a40bab7b3"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 671, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8312d4fd-fe6d-4001-a037-cde12000161d"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 778, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("8db9e713-d6f0-4f34-b348-c7da0c1a51d6"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 20, DateTimeKind.Utc).AddTicks(780));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("9ae2c2f3-4ab1-4512-9832-7649d5ff61d8"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 847, DateTimeKind.Utc).AddTicks(1630));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("b0f3e786-f68b-46fe-8b18-f4a6e1150804"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 122, DateTimeKind.Utc).AddTicks(6450));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("c91c99ca-fffd-42a5-9e6e-fa67d3c0f762"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 985, DateTimeKind.Utc).AddTicks(880));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("cd9e4f1f-8edd-4488-b0da-256521a720e8"),
                columns: new[] { "DateCreated", "VerificationCode", "VerificationCodeExpiration" },
                values: new object[] { new DateTime(2025, 7, 14, 10, 53, 35, 88, DateTimeKind.Utc).AddTicks(210), "250744", new DateTime(2025, 7, 15, 10, 53, 35, 88, DateTimeKind.Utc).AddTicks(210) });

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("dbf24f67-7457-47c3-a2af-a117d8e90b00"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 35, 157, DateTimeKind.Utc).AddTicks(4690));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("df0a8813-0938-42a6-ac84-26298701f456"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 916, DateTimeKind.Utc).AddTicks(8630));

            migrationBuilder.UpdateData(
                table: "UserLog",
                keyColumn: "IdUserLog",
                keyValue: new Guid("e7653083-1497-4aa0-a56b-dec32a61d71f"),
                column: "DateCreated",
                value: new DateTime(2025, 7, 14, 10, 53, 34, 812, DateTimeKind.Utc).AddTicks(3830));
        }
    }
}
