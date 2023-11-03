using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionGroupItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_Distribution_DistributionId",
                table: "DistributionGroupItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_TelegramChats_TelegramChatId",
                table: "DistributionGroupItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_ViberChats_ViberChatId",
                table: "DistributionGroupItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionGroupItems",
                table: "DistributionGroupItems");

            migrationBuilder.RenameTable(
                name: "DistributionGroupItems",
                newName: "DistributionItems");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionGroupItems_ViberChatId",
                table: "DistributionItems",
                newName: "IX_DistributionItems_ViberChatId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionGroupItems_TelegramChatId",
                table: "DistributionItems",
                newName: "IX_DistributionItems_TelegramChatId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionGroupItems_DistributionSettingsId",
                table: "DistributionItems",
                newName: "IX_DistributionItems_DistributionSettingsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionItems",
                table: "DistributionItems",
                columns: new[] { "DistributionId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionItems",
                column: "DistributionSettingsId",
                principalTable: "DistributionSettingsRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionItems_Distribution_DistributionId",
                table: "DistributionItems",
                column: "DistributionId",
                principalTable: "Distribution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionItems_TelegramChats_TelegramChatId",
                table: "DistributionItems",
                column: "TelegramChatId",
                principalTable: "TelegramChats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionItems_ViberChats_ViberChatId",
                table: "DistributionItems",
                column: "ViberChatId",
                principalTable: "ViberChats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionItems_Distribution_DistributionId",
                table: "DistributionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionItems_TelegramChats_TelegramChatId",
                table: "DistributionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionItems_ViberChats_ViberChatId",
                table: "DistributionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionItems",
                table: "DistributionItems");

            migrationBuilder.RenameTable(
                name: "DistributionItems",
                newName: "DistributionGroupItems");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionItems_ViberChatId",
                table: "DistributionGroupItems",
                newName: "IX_DistributionGroupItems_ViberChatId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionItems_TelegramChatId",
                table: "DistributionGroupItems",
                newName: "IX_DistributionGroupItems_TelegramChatId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionItems_DistributionSettingsId",
                table: "DistributionGroupItems",
                newName: "IX_DistributionGroupItems_DistributionSettingsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionGroupItems",
                table: "DistributionGroupItems",
                columns: new[] { "DistributionId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId",
                principalTable: "DistributionSettingsRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_Distribution_DistributionId",
                table: "DistributionGroupItems",
                column: "DistributionId",
                principalTable: "Distribution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_TelegramChats_TelegramChatId",
                table: "DistributionGroupItems",
                column: "TelegramChatId",
                principalTable: "TelegramChats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_ViberChats_ViberChatId",
                table: "DistributionGroupItems",
                column: "ViberChatId",
                principalTable: "ViberChats",
                principalColumn: "Id");
        }
    }
}
