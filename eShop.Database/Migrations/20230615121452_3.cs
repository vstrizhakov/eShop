using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Compositions_CompositionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompositionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompositionId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "CompositionProduct",
                columns: table => new
                {
                    CompositionsId = table.Column<string>(type: "TEXT", nullable: false),
                    ProductsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositionProduct", x => new { x.CompositionsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CompositionProduct_Compositions_CompositionsId",
                        column: x => x.CompositionsId,
                        principalTable: "Compositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompositionProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompositionProduct_ProductsId",
                table: "CompositionProduct",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompositionProduct");

            migrationBuilder.AddColumn<string>(
                name: "CompositionId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompositionId",
                table: "Products",
                column: "CompositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Compositions_CompositionId",
                table: "Products",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "Id");
        }
    }
}
