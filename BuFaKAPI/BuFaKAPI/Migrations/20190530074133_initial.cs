namespace BuFaKAPI.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth",
                columns: table => new
                {
                    TokenID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApiKey = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<string>(nullable: true),
                    ConferenceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth", x => x.TokenID);
                });

            migrationBuilder.CreateTable(
                name: "Conference",
                columns: table => new
                {
                    ConferenceID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateStart = table.Column<string>(nullable: true),
                    DateEnd = table.Column<string>(nullable: true),
                    CouncilID = table.Column<int>(nullable: false),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false),
                    ConferenceApplicationPhase = table.Column<bool>(nullable: false, defaultValue: false),
                    WorkshopApplicationPhase = table.Column<bool>(nullable: false, defaultValue: false),
                    WorkshopSuggestionPhase = table.Column<bool>(nullable: false, defaultValue: false),
                    AttendeeCost = table.Column<string>(nullable: true),
                    AlumnusCost = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conference", x => x.ConferenceID);
                });

            migrationBuilder.CreateTable(
                name: "Council",
                columns: table => new
                {
                    CouncilID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    NameShort = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    University = table.Column<string>(nullable: true),
                    UniversityShort = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Council", x => x.CouncilID);
                });

            migrationBuilder.CreateTable(
                name: "Sensible",
                columns: table => new
                {
                    SensibleID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceID = table.Column<int>(nullable: false),
                    Timestamp = table.Column<string>(nullable: true),
                    BuFaKCount = table.Column<int>(nullable: false),
                    UID = table.Column<string>(nullable: true),
                    EatingPreferences = table.Column<string>(nullable: true),
                    Intolerances = table.Column<string>(nullable: true),
                    SleepingPreferences = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    ExtraNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensible", x => x.SensibleID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CouncilID = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    UID = table.Column<string>(nullable: false),
                    ConferenceID = table.Column<int>(nullable: false),
                    ValidUntil = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => new { x.UID, x.ConferenceID });
                    table.ForeignKey(
                        name: "FK_Administrator_Conference_ConferenceID",
                        column: x => x.ConferenceID,
                        principalTable: "Conference",
                        principalColumn: "ConferenceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administrator_User_UID",
                        column: x => x.UID,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conference_Application",
                columns: table => new
                {
                    ConferenceID = table.Column<int>(nullable: false),
                    ApplicantUID = table.Column<string>(nullable: false),
                    UserUID = table.Column<string>(nullable: true),
                    SensibleID = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsAlumnus = table.Column<bool>(nullable: false),
                    IsBuFaKCouncil = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Timestamp = table.Column<string>(nullable: true),
                    IsHelper = table.Column<bool>(nullable: false),
                    Hotel = table.Column<string>(nullable: true),
                    Room = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conference_Application", x => new { x.ConferenceID, x.ApplicantUID });
                    table.ForeignKey(
                        name: "FK_Conference_Application_Conference_ConferenceID",
                        column: x => x.ConferenceID,
                        principalTable: "Conference",
                        principalColumn: "ConferenceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conference_Application_Sensible_SensibleID",
                        column: x => x.SensibleID,
                        principalTable: "Sensible",
                        principalColumn: "SensibleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conference_Application_User_UserUID",
                        column: x => x.UserUID,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    HistoryID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OldValue = table.Column<string>(nullable: true),
                    ResponsibleUID = table.Column<string>(nullable: true),
                    UserUID = table.Column<string>(nullable: true),
                    HistoryType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_History_User_UserUID",
                        column: x => x.UserUID,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Workshop",
                columns: table => new
                {
                    WorkshopID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameShort = table.Column<string>(nullable: true),
                    Overview = table.Column<string>(nullable: true),
                    MaxVisitors = table.Column<int>(nullable: false),
                    Difficulty = table.Column<string>(nullable: true),
                    HostUID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    UserUID = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    Start = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    MaterialNote = table.Column<string>(nullable: true),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshop", x => x.WorkshopID);
                    table.ForeignKey(
                        name: "FK_Workshop_Conference_ConferenceID",
                        column: x => x.ConferenceID,
                        principalTable: "Conference",
                        principalColumn: "ConferenceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workshop_User_UserUID",
                        column: x => x.UserUID,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Workshop_Application",
                columns: table => new
                {
                    WorkshopID = table.Column<int>(nullable: false),
                    ApplicantUID = table.Column<string>(nullable: false),
                    IsHelper = table.Column<bool>(nullable: false),
                    UserUID = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Invalid = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshop_Application", x => new { x.WorkshopID, x.ApplicantUID });
                    table.ForeignKey(
                        name: "FK_Workshop_Application_User_UserUID",
                        column: x => x.UserUID,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Workshop_Application_Workshop_WorkshopID",
                        column: x => x.WorkshopID,
                        principalTable: "Workshop",
                        principalColumn: "WorkshopID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_ConferenceID",
                table: "Administrator",
                column: "ConferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_Application_SensibleID",
                table: "Conference_Application",
                column: "SensibleID");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_Application_UserUID",
                table: "Conference_Application",
                column: "UserUID");

            migrationBuilder.CreateIndex(
                name: "IX_History_UserUID",
                table: "History",
                column: "UserUID");

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_ConferenceID",
                table: "Workshop",
                column: "ConferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_UserUID",
                table: "Workshop",
                column: "UserUID");

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_Application_UserUID",
                table: "Workshop_Application",
                column: "UserUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Auth");

            migrationBuilder.DropTable(
                name: "Conference_Application");

            migrationBuilder.DropTable(
                name: "Council");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Workshop_Application");

            migrationBuilder.DropTable(
                name: "Sensible");

            migrationBuilder.DropTable(
                name: "Workshop");

            migrationBuilder.DropTable(
                name: "Conference");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
