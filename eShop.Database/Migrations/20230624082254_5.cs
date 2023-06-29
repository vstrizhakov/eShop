using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TelegramUsers_LinkedUserId",
                table: "TelegramUsers");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_LinkedUserId",
                table: "TelegramUsers",
                column: "LinkedUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TelegramUsers_LinkedUserId",
                table: "TelegramUsers");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_LinkedUserId",
                table: "TelegramUsers",
                column: "LinkedUserId");
        }
    }
}
