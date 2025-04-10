using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizTask.Migrations
{
    /// <inheritdoc />
    public partial class Candi_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CandidateDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CandidateDetails");
        }
    }
}
