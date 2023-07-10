using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class AccountsTableIdentityUserIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Accounts");
        }
    }
}
