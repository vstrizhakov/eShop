using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionItemsTableAccountIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "DistributionItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DistributionItems_AccountId",
                table: "DistributionItems",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionItems_Accounts_AccountId",
                table: "DistributionItems",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionItems_Accounts_AccountId",
                table: "DistributionItems");

            migrationBuilder.DropIndex(
                name: "IX_DistributionItems_AccountId",
                table: "DistributionItems");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "DistributionItems");
        }
    }
}
