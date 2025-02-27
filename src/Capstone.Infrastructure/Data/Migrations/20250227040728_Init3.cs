using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Capstone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2fe7af91-5983-4ae6-a6a3-5fba4f3c8a25", null, "User", "USER" },
                    { "423b4dca-f69a-4cd3-9367-ed776ac94d48", null, "Admin", "ADMIN" },
                    { "a84db60a-88f3-462d-8312-0a1bc7feb8f3", null, "Rescue", "RESCUE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fe7af91-5983-4ae6-a6a3-5fba4f3c8a25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "423b4dca-f69a-4cd3-9367-ed776ac94d48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a84db60a-88f3-462d-8312-0a1bc7feb8f3");
        }
    }
}
