using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class CompositionsTableRenamedToAnnounces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Compositions_CompositionId",
                table: "CompositionImages");

            migrationBuilder.DropTable(
                name: "CompositionProduct");

            migrationBuilder.RenameColumn(
                name: "CompositionId",
                table: "CompositionImages",
                newName: "AnnounceId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_CompositionId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_AnnounceId");

            migrationBuilder.CreateTable(
                name: "AnnounceProduct",
                columns: table => new
                {
                    AnnouncesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnounceProduct", x => new { x.AnnouncesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_AnnounceProduct_Compositions_AnnouncesId",
                        column: x => x.AnnouncesId,
                        principalTable: "Compositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnounceProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_AnnounceProduct_ProductsId",
                table: "AnnounceProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Compositions_AnnounceId",
                table: "CompositionImages",
                column: "AnnounceId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Compositions_AnnounceId",
                table: "CompositionImages");

            migrationBuilder.DropTable(
                name: "AnnounceProduct");

            migrationBuilder.RenameColumn(
                name: "AnnounceId",
                table: "CompositionImages",
                newName: "CompositionId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_AnnounceId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_CompositionId");

            migrationBuilder.CreateTable(
                name: "CompositionProduct",
                columns: table => new
                {
                    CompositionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(2145), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(2140), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(2143), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(1981), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(1964), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 29, 8, 10, 44, 422, DateTimeKind.Unspecified).AddTicks(1996), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_CompositionProduct_ProductsId",
                table: "CompositionProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Compositions_CompositionId",
                table: "CompositionImages",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
