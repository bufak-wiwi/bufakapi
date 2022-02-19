using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class addTravelInfos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Travel",
                columns: table => new
                {
                    TravelID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceID = table.Column<int>(nullable: false),
                    UID = table.Column<string>(nullable: true),
                    Transportation = table.Column<string>(nullable: true),
                    ParkingSpace = table.Column<bool>(nullable: false),
                    ArrivalTimestamp = table.Column<string>(nullable: true),
                    ArrivalPlace = table.Column<string>(nullable: true),
                    DepartureTimestamp = table.Column<string>(nullable: true),
                    ExtraNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travel", x => x.TravelID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Travel");
        }
    }
}
