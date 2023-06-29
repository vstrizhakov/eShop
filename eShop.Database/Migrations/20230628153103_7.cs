using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUsers_AspNetUsers_LinkedUserId",
                table: "TelegramUsers");

            migrationBuilder.RenameColumn(
                name: "LinkedUserId",
                table: "TelegramUsers",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUsers_LinkedUserId",
                table: "TelegramUsers",
                newName: "IX_TelegramUsers_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ViberUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUsers_AspNetUsers_OwnerId",
                table: "TelegramUsers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ViberUsers_AspNetUsers_OwnerId",
                table: "ViberUsers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUsers_AspNetUsers_OwnerId",
                table: "TelegramUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ViberUsers_AspNetUsers_OwnerId",
                table: "ViberUsers");

            migrationBuilder.DropIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ViberUsers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "TelegramUsers",
                newName: "LinkedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUsers_OwnerId",
                table: "TelegramUsers",
                newName: "IX_TelegramUsers_LinkedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUsers_AspNetUsers_LinkedUserId",
                table: "TelegramUsers",
                column: "LinkedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
