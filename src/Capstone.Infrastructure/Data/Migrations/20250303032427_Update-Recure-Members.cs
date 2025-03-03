using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecureMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RescueMemberIds");

            migrationBuilder.CreateTable(
                name: "RescueMembers",
                columns: table => new
                {
                    RescueMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RescueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RescueMemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Việt Nam"),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueMembers", x => new { x.RescueMemberId, x.RescueId });
                    table.ForeignKey(
                        name: "FK_RescueMembers_Rescues_RescueId",
                        column: x => x.RescueId,
                        principalTable: "Rescues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RescueMembers_RescueId",
                table: "RescueMembers",
                column: "RescueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RescueMembers");

            migrationBuilder.CreateTable(
                name: "RescueMemberIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RescueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RescueMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueMemberIds", x => new { x.Id, x.RescueId });
                    table.ForeignKey(
                        name: "FK_RescueMemberIds_Rescues_RescueId",
                        column: x => x.RescueId,
                        principalTable: "Rescues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RescueMemberIds_RescueId",
                table: "RescueMemberIds",
                column: "RescueId");
        }
    }
}
