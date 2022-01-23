using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class addInfoTextAndLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InformationTextConferenceApplication",
                table: "Conference",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InformationTextWorkshopSuggestion",
                table: "Conference",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkParticipantAgreement",
                table: "Conference",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InformationTextConferenceApplication",
                table: "Conference");

            migrationBuilder.DropColumn(
                name: "InformationTextWorkshopSuggestion",
                table: "Conference");

            migrationBuilder.DropColumn(
                name: "LinkParticipantAgreement",
                table: "Conference");
        }
    }
}
