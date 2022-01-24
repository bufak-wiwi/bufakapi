using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class addWorkshopFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Workshop",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkshopDurations",
                table: "Conference",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkshopTopics",
                table: "Conference",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Workshop");

            migrationBuilder.DropColumn(
                name: "WorkshopDurations",
                table: "Conference");

            migrationBuilder.DropColumn(
                name: "WorkshopTopics",
                table: "Conference");
        }
    }
}
