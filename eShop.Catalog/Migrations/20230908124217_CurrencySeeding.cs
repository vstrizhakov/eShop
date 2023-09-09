using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class CurrencySeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"), new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(2074), new TimeSpan(0, 0, 0, 0, 0)), "EUR" },
                    { new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"), new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(2013), new TimeSpan(0, 0, 0, 0, 0)), "UAH" },
                    { new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"), new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(2016), new TimeSpan(0, 0, 0, 0, 0)), "USD" }
                });

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(1854), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(1837), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 12, 42, 17, 142, DateTimeKind.Unspecified).AddTicks(1856), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4879), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4862), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4881), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
