using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizTask.Migrations
{
    /// <inheritdoc />
    public partial class EmailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "TestLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Candidate_TestLink",
                table: "TestLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "EmailDetails",
                columns: table => new
                {
                    EmailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestLinkId = table.Column<int>(type: "int", nullable: false),
                    Confirmed_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailDetails", x => x.EmailID);
                    table.ForeignKey(
                        name: "FK_EmailDetails_TestLinks_TestLinkId",
                        column: x => x.TestLinkId,
                        principalTable: "TestLinks",
                        principalColumn: "TestLinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailDetails_TestLinkId",
                table: "EmailDetails",
                column: "TestLinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "TestLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Candidate_TestLink",
                table: "TestLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
