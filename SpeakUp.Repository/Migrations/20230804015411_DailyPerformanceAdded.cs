using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakUp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DailyPerformanceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyPerformances",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WordsGuessedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WordsLearnedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MinutesSpentLearning = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPerformances", x => new { x.UserId, x.Date });
                    table.ForeignKey(
                        name: "FK_DailyPerformances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyPerformances");
        }
    }
}
