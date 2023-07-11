using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ViberChats",
                table: "ViberChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramChats",
                table: "TelegramChats");

            migrationBuilder.RenameColumn(
                name: "ViberUserId",
                table: "ViberChats",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TelegramChatId",
                table: "TelegramChats",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViberChats",
                table: "ViberChats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramChats",
                table: "TelegramChats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DistributionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistributionGroupItems",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TelegramChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ViberChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionGroupItems", x => new { x.GroupId, x.Id });
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_DistributionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DistributionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_TelegramChats_TelegramChatId",
                        column: x => x.TelegramChatId,
                        principalTable: "TelegramChats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_ViberChats_ViberChatId",
                        column: x => x.ViberChatId,
                        principalTable: "ViberChats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChats_AccountId",
                table: "TelegramChats",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_TelegramChatId",
                table: "DistributionGroupItems",
                column: "TelegramChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_ViberChatId",
                table: "DistributionGroupItems",
                column: "ViberChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "DistributionGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ViberChats",
                table: "ViberChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramChats",
                table: "TelegramChats");

            migrationBuilder.DropIndex(
                name: "IX_TelegramChats_AccountId",
                table: "TelegramChats");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ViberChats",
                newName: "ViberUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TelegramChats",
                newName: "TelegramChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViberChats",
                table: "ViberChats",
                columns: new[] { "AccountId", "ViberUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramChats",
                table: "TelegramChats",
                columns: new[] { "AccountId", "TelegramChatId" });
        }
    }
}
