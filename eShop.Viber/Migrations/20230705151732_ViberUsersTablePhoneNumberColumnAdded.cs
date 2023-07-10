using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Viber.Migrations
{
    /// <inheritdoc />
    public partial class ViberUsersTablePhoneNumberColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ViberUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ViberUsers");
        }
    }
}
