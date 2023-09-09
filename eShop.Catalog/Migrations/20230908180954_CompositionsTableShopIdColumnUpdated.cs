using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class CompositionsTableShopIdColumnUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Compositions_ShopId",
                table: "Compositions",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Shops_ShopId",
                table: "Compositions",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Shops_ShopId",
                table: "Compositions");

            migrationBuilder.DropIndex(
                name: "IX_Compositions_ShopId",
                table: "Compositions");
        }
    }
}
