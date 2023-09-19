using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakUp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class GrammarAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrammarId",
                table: "Words",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Decks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Decks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Grammars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrammarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grammars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grammars_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_GrammarId",
                table: "Words",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_Grammars_DeckId",
                table: "Grammars",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Grammars_GrammarId",
                table: "Words",
                column: "GrammarId",
                principalTable: "Grammars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Grammars_GrammarId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "Grammars");

            migrationBuilder.DropIndex(
                name: "IX_Words_GrammarId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "GrammarId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Decks");
        }
    }
}
