using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class AccountsTableViberUserIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ViberUserId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViberUserId",
                table: "Accounts");
        }
    }
}
