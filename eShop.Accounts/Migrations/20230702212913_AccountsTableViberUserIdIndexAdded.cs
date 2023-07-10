using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class AccountsTableViberUserIdIndexAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ViberUserId",
                table: "Accounts",
                column: "ViberUserId",
                unique: true,
                filter: "[ViberUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_ViberUserId",
                table: "Accounts");
        }
    }
}
