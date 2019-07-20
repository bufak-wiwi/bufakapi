using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class superadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperAdmin",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperAdmin",
                table: "User");
        }
    }
}
