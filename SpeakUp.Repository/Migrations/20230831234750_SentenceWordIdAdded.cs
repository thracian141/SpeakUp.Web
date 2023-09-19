using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakUp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SentenceWordIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sentences_Decks_DeckId",
                table: "Sentences");

            migrationBuilder.RenameColumn(
                name: "DeckId",
                table: "Sentences",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_Sentences_DeckId",
                table: "Sentences",
                newName: "IX_Sentences_WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sentences_Words_WordId",
                table: "Sentences",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sentences_Words_WordId",
                table: "Sentences");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "Sentences",
                newName: "DeckId");

            migrationBuilder.RenameIndex(
                name: "IX_Sentences_WordId",
                table: "Sentences",
                newName: "IX_Sentences_DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sentences_Decks_DeckId",
                table: "Sentences",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
