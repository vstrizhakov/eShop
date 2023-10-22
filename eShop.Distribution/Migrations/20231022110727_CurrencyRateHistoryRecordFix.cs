using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRateHistoryRecordFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecord_DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord");

            migrationBuilder.DropColumn(
                name: "DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord",
                column: "DistributionSettingsHistoryRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecord_DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord",
                column: "DistributionSettingsHistoryRecordId",
                principalTable: "DistributionSettingsHistoryRecord",
                principalColumn: "Id");
        }
    }
}
