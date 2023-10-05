using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class ShopSettingsTablePreferredShopsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopShopSettings",
                columns: table => new
                {
                    PreferredShopsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopShopSettings", x => new { x.PreferredShopsId, x.ShopSettingsId });
                    table.ForeignKey(
                        name: "FK_ShopShopSettings_ShopSettings_ShopSettingsId",
                        column: x => x.ShopSettingsId,
                        principalTable: "ShopSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopShopSettings_Shops_PreferredShopsId",
                        column: x => x.PreferredShopsId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopShopSettings_ShopSettingsId",
                table: "ShopShopSettings",
                column: "ShopSettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopShopSettings");
        }
    }
}
