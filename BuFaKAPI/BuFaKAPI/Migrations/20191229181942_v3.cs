using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddFields",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicProfile",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AddFields",
                table: "Conference",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<string>(nullable: false),
                    ConferenceID = table.Column<string>(nullable: true),
                    Start = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PlaceID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsID = table.Column<string>(nullable: false),
                    ConferenceID = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Start = table.Column<string>(nullable: true),
                    End = table.Column<string>(nullable: true),
                    PictureLink = table.Column<string>(nullable: true),
                    Important = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsID);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    PlaceID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.PlaceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropColumn(
                name: "AddFields",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PublicProfile",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AddFields",
                table: "Conference");
        }
    }
}
