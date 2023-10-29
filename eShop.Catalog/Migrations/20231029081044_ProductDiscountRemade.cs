using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class ProductDiscountRemade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sale",
                table: "Products");

            migrationBuilder.AddColumn<double>(
                name: "DiscountedValue",
                table: "ProductPrices",
                type: "float",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedValue",
                table: "ProductPrices");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sale",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3432), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3427), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3430), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3288), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3268), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 25, 20, 54, 9, 87, DateTimeKind.Unspecified).AddTicks(3291), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
