using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Viber.Migrations
{
    /// <inheritdoc />
    public partial class ViberUsersTableRegistrationProviderIdColumnRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationProviderId",
                table: "ViberUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationProviderId",
                table: "ViberUsers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
