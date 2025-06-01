using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixbugv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PointInCorrect",
                table: "ExamVersionQuestions",
                newName: "PointPerInCorrect");

            migrationBuilder.RenameColumn(
                name: "PointCorrect",
                table: "ExamVersionQuestions",
                newName: "PointPerCorrect");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PointPerInCorrect",
                table: "ExamVersionQuestions",
                newName: "PointInCorrect");

            migrationBuilder.RenameColumn(
                name: "PointPerCorrect",
                table: "ExamVersionQuestions",
                newName: "PointCorrect");
        }
    }
}
