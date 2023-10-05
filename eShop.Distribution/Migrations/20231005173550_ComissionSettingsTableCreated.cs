using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class ComissionSettingsTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComissionSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Show = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComissionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComissionSettings_DistributionSettings_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettings",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComissionSettings_DistributionSettingsId",
                table: "ComissionSettings",
                column: "DistributionSettingsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComissionSettings");
        }
    }
}
