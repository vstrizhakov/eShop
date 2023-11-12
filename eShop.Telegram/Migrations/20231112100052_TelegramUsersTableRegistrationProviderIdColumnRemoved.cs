using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUsersTableRegistrationProviderIdColumnRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationProviderId",
                table: "TelegramUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationProviderId",
                table: "TelegramUsers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
