using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class ProductsTableSaleColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7516), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7510), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7514), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7288), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7269), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 8, 26, 737, DateTimeKind.Unspecified).AddTicks(7290), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sale",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7603), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7598), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7601), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7366), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7349), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 14, 6, 56, 8, DateTimeKind.Unspecified).AddTicks(7368), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
