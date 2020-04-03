using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuFaKAPI.Migrations
{
    public partial class addVotings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotingAnswer",
                columns: table => new
                {
                    AnswerID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionID = table.Column<int>(nullable: false),
                    CouncilID = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Vote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingAnswer", x => x.AnswerID);
                    table.ForeignKey(
                        name: "FK_VotingAnswer_Council_CouncilID",
                        column: x => x.CouncilID,
                        principalTable: "Council",
                        principalColumn: "CouncilID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VotingMajority",
                columns: table => new
                {
                    MajorityID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Secret = table.Column<string>(nullable: true),
                    Calculation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingMajority", x => x.MajorityID);
                });

            migrationBuilder.CreateTable(
                name: "VotingQuestion",
                columns: table => new
                {
                    QuestionID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceID = table.Column<int>(nullable: false),
                    MajorityID = table.Column<int>(nullable: false),
                    QuestionText = table.Column<string>(nullable: true),
                    ArrivedCouncilCount = table.Column<int>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    Vote = table.Column<string>(nullable: true),
                    ResolvedOn = table.Column<DateTime>(nullable: false),
                    SumYes = table.Column<int>(nullable: false),
                    SumNo = table.Column<int>(nullable: false),
                    SumAbstention = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingQuestion", x => x.QuestionID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotingAnswer_CouncilID",
                table: "VotingAnswer",
                column: "CouncilID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotingAnswer");

            migrationBuilder.DropTable(
                name: "VotingMajority");

            migrationBuilder.DropTable(
                name: "VotingQuestion");
        }
    }
}
