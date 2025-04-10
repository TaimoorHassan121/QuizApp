using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizTask.Migrations
{
    /// <inheritdoc />
    public partial class emailupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Created_By",
                table: "EmailDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "EmailDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "EmailDetails");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "EmailDetails");
        }
    }
}
