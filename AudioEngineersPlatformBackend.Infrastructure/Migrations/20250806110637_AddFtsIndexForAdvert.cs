using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFtsIndexForAdvert : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX on dbo.Advert; DROP FULLTEXT CATALOG AdvertFTC;", true);
        }
    }
}
