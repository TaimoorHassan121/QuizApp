using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizTask.Migrations
{
    /// <inheritdoc />
    public partial class SecondMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_Reg_No = table.Column<int>(type: "int", nullable: false),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Mobile_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Passward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Status = table.Column<bool>(type: "bit", nullable: false),
                    User_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AdminUsers_AspNetRoles_IdentityRoleId",
                        column: x => x.IdentityRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdminUsers_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateDetails",
                columns: table => new
                {
                    CandidateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelID = table.Column<int>(type: "int", nullable: false),
                    Mobile_No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Candidate_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateDetails", x => x.CandidateID);
                    table.ForeignKey(
                        name: "FK_CandidateDetails_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CandidateDetails_CareerLevels_LevelID",
                        column: x => x.LevelID,
                        principalTable: "CareerLevels",
                        principalColumn: "LevelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizLogs",
                columns: table => new
                {
                    QuizLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CareelLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    option2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    option3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    option4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    option5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizStatus = table.Column<bool>(type: "bit", nullable: false),
                    Quiz_AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quiz_UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizLogs", x => x.QuizLogId);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionAnswers",
                columns: table => new
                {
                    QuizID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizQno = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelID = table.Column<int>(type: "int", nullable: false),
                    QuizStatus = table.Column<bool>(type: "bit", nullable: false),
                    QuizDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionAnswers", x => x.QuizID);
                    table.ForeignKey(
                        name: "FK_QuizQuestionAnswers_CareerLevels_LevelID",
                        column: x => x.LevelID,
                        principalTable: "CareerLevels",
                        principalColumn: "LevelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAtmp_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateID = table.Column<int>(type: "int", nullable: false),
                    Quiz_Attempts = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAtmp_ID);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_CandidateDetails_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "CandidateDetails",
                        principalColumn: "CandidateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestLinks",
                columns: table => new
                {
                    TestLinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateID = table.Column<int>(type: "int", nullable: false),
                    Candidate_TestLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestLinks", x => x.TestLinkId);
                    table.ForeignKey(
                        name: "FK_TestLinks_CandidateDetails_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "CandidateDetails",
                        principalColumn: "CandidateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizOptions",
                columns: table => new
                {
                    OptID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionNo = table.Column<int>(type: "int", nullable: false),
                    QuizID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizOptions", x => x.OptID);
                    table.ForeignKey(
                        name: "FK_QuizOptions_QuizQuestionAnswers_QuizID",
                        column: x => x.QuizID,
                        principalTable: "QuizQuestionAnswers",
                        principalColumn: "QuizID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_IdentityRoleId",
                table: "AdminUsers",
                column: "IdentityRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_IdentityUserId",
                table: "AdminUsers",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDetails_IdentityUserId",
                table: "CandidateDetails",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDetails_LevelID",
                table: "CandidateDetails",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_CandidateID",
                table: "QuizAttempts",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizOptions_QuizID",
                table: "QuizOptions",
                column: "QuizID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_LevelID",
                table: "QuizQuestionAnswers",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_TestLinks_CandidateID",
                table: "TestLinks",
                column: "CandidateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "QuizLogs");

            migrationBuilder.DropTable(
                name: "QuizOptions");

            migrationBuilder.DropTable(
                name: "TestLinks");

            migrationBuilder.DropTable(
                name: "QuizQuestionAnswers");

            migrationBuilder.DropTable(
                name: "CandidateDetails");
        }
    }
}
