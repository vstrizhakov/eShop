using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramRequests");

            migrationBuilder.CreateTable(
                name: "TelegramChatSettings",
                columns: table => new
                {
                    TelegramChatId = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramChatSettings", x => new { x.TelegramChatId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_TelegramChatSettings_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TelegramChatSettings_TelegramChats_TelegramChatId",
                        column: x => x.TelegramChatId,
                        principalTable: "TelegramChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChatSettings_OwnerId",
                table: "TelegramChatSettings",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChatSettings_TelegramChatId",
                table: "TelegramChatSettings",
                column: "TelegramChatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramChatSettings");

            migrationBuilder.CreateTable(
                name: "TelegramRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramRequests_TelegramUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramRequests_UserId",
                table: "TelegramRequests",
                column: "UserId");
        }
    }
}
