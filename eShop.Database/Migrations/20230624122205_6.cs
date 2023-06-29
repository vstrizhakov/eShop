using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_AspNetUsers_OwnerId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_OwnerId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Currencies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Currencies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_OwnerId",
                table: "Currencies",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_AspNetUsers_OwnerId",
                table: "Currencies",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
