using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class nameForConference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Conference",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Conference");
        }
    }
}
