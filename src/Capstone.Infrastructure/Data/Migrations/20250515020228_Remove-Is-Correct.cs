using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "ParticipantAnswers");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "ParticipantAnswers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ParticipantAnswers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "ParticipantAnswers",
                type: "bit",
                nullable: true);
        }
    }
}
