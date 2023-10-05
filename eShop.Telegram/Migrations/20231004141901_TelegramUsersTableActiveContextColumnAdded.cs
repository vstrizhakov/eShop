using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUsersTableActiveContextColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveContext",
                table: "TelegramUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveContext",
                table: "TelegramUsers");
        }
    }
}
