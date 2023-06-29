using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers");

            migrationBuilder.CreateTable(
                name: "ViberChatSettings",
                columns: table => new
                {
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    ViberUserId = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViberChatSettings", x => new { x.OwnerId, x.ViberUserId });
                    table.ForeignKey(
                        name: "FK_ViberChatSettings_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViberChatSettings_ViberUsers_ViberUserId",
                        column: x => x.ViberUserId,
                        principalTable: "ViberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViberChatSettings_OwnerId",
                table: "ViberChatSettings",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViberChatSettings_ViberUserId",
                table: "ViberChatSettings",
                column: "ViberUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViberChatSettings");

            migrationBuilder.DropIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers");

            migrationBuilder.CreateIndex(
                name: "IX_ViberUsers_OwnerId",
                table: "ViberUsers",
                column: "OwnerId");
        }
    }
}
