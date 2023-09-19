using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakUp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CoursesInDecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCourse",
                table: "Decks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCourse",
                table: "Decks");
        }
    }
}
