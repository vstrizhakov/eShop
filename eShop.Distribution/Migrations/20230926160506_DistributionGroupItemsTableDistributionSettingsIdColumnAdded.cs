using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionGroupItemsTableDistributionSettingsIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DistributionSettingsId",
                table: "DistributionGroupItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettings_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId",
                principalTable: "DistributionSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettings_DistributionSettingsId",
                table: "DistributionGroupItems");

            migrationBuilder.DropIndex(
                name: "IX_DistributionGroupItems_DistributionSettingsId",
                table: "DistributionGroupItems");

            migrationBuilder.DropColumn(
                name: "DistributionSettingsId",
                table: "DistributionGroupItems");
        }
    }
}
