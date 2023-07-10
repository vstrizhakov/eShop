using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUserTablePhoneNumberColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "TelegramUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "TelegramUsers");
        }
    }
}
