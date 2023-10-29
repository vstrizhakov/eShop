using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class ComissionAndShopSettingsRecordTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShopSettingsRecordId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowSales",
                table: "DistributionSettingsRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ComissionSettingsRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComissionSettingsRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComissionSettingsRecord_DistributionSettingsRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopSettingsRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSettingsRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopSettingsRecord_DistributionSettingsRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ShopSettingsRecordId",
                table: "Shops",
                column: "ShopSettingsRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ComissionSettingsRecord_DistributionSettingsId",
                table: "ComissionSettingsRecord",
                column: "DistributionSettingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopSettingsRecord_DistributionSettingsId",
                table: "ShopSettingsRecord",
                column: "DistributionSettingsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_ShopSettingsRecord_ShopSettingsRecordId",
                table: "Shops",
                column: "ShopSettingsRecordId",
                principalTable: "ShopSettingsRecord",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_ShopSettingsRecord_ShopSettingsRecordId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "ComissionSettingsRecord");

            migrationBuilder.DropTable(
                name: "ShopSettingsRecord");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ShopSettingsRecordId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ShopSettingsRecordId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ShowSales",
                table: "DistributionSettingsRecord");
        }
    }
}
