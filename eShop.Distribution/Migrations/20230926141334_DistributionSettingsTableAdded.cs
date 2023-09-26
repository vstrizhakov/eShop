using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionSettingsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistributionSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionSettings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributionSettings_Currencies_PreferredCurrencyId",
                        column: x => x.PreferredCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettings_AccountId",
                table: "DistributionSettings",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettings_PreferredCurrencyId",
                table: "DistributionSettings",
                column: "PreferredCurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionSettings");
        }
    }
}
