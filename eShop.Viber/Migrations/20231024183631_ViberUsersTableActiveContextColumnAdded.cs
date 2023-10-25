using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Viber.Migrations
{
    /// <inheritdoc />
    public partial class ViberUsersTableActiveContextColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveContext",
                table: "ViberUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveContext",
                table: "ViberUsers");
        }
    }
}
