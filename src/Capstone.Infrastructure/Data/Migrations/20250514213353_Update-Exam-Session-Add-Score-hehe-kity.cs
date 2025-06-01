using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamSessionAddScorehehekity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Right",
                table: "MatchingQuestionsMatchingPairs",
                newName: "RightValue");

            migrationBuilder.RenameColumn(
                name: "Left",
                table: "MatchingQuestionsMatchingPairs",
                newName: "LeftValue");

            migrationBuilder.AddColumn<Guid>(
                name: "LeftId",
                table: "MatchingQuestionsMatchingPairs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.AddColumn<Guid>(
                name: "RightId",
                table: "MatchingQuestionsMatchingPairs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftId",
                table: "MatchingQuestionsMatchingPairs");

            migrationBuilder.DropColumn(
                name: "RightId",
                table: "MatchingQuestionsMatchingPairs");

            migrationBuilder.RenameColumn(
                name: "RightValue",
                table: "MatchingQuestionsMatchingPairs",
                newName: "Right");

            migrationBuilder.RenameColumn(
                name: "LeftValue",
                table: "MatchingQuestionsMatchingPairs",
                newName: "Left");
        }
    }
}
