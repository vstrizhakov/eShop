using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsHistoryRecord_DistributionSettingsId",
                table: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "CurrencyRateHistoryRecord");

            migrationBuilder.DropTable(
                name: "DistributionSettingsHistoryRecord");

            migrationBuilder.CreateTable(
                name: "DistributionSettingsRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSettingsRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionSettingsRecord_Currencies_PreferredCurrencyId",
                        column: x => x.PreferredCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRateRecord",
                columns: table => new
                {
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateRecord", x => new { x.DistributionSettingsId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrencyRateRecord_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyRateRecord_DistributionSettingsRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateRecord_CurrencyId",
                table: "CurrencyRateRecord",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettingsRecord_PreferredCurrencyId",
                table: "DistributionSettingsRecord",
                column: "PreferredCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId",
                principalTable: "DistributionSettingsRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsRecord_DistributionSettingsId",
                table: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "CurrencyRateRecord");

            migrationBuilder.DropTable(
                name: "DistributionSettingsRecord");

            migrationBuilder.CreateTable(
                name: "DistributionSettingsHistoryRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSettingsHistoryRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionSettingsHistoryRecord_Currencies_PreferredCurrencyId",
                        column: x => x.PreferredCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRateHistoryRecord",
                columns: table => new
                {
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateHistoryRecord", x => new { x.DistributionSettingsId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistoryRecord_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsHistoryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateHistoryRecord_CurrencyId",
                table: "CurrencyRateHistoryRecord",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettingsHistoryRecord_PreferredCurrencyId",
                table: "DistributionSettingsHistoryRecord",
                column: "PreferredCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_DistributionSettingsHistoryRecord_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId",
                principalTable: "DistributionSettingsHistoryRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
