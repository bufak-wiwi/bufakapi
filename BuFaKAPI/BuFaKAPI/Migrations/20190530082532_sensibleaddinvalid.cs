namespace BuFaKAPI.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class sensibleaddinvalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invalid",
                table: "Sensible",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invalid",
                table: "Sensible");
        }
    }
}
