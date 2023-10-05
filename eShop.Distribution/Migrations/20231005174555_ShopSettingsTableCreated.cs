using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class ShopSettingsTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Filter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopSettings_DistributionSettings_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettings",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopSettings_DistributionSettingsId",
                table: "ShopSettings",
                column: "DistributionSettingsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopSettings");
        }
    }
}
