using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("69d06081-f197-4823-8248-eb6c60cb73a4"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("6fc5e363-a92e-4412-9dbb-c84841e06f91"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("a05f6d6b-ccb2-43fd-8694-eceafdf0ab31"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("a3b279ea-2a65-4996-b0b5-ec242c90ebb2"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("d5d120a1-dba7-4e87-a4e1-9670a365dd2d"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("e4021ee6-1568-448c-a95b-ac76191f235b"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("69d06081-f197-4823-8248-eb6c60cb73a4"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1875), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("6fc5e363-a92e-4412-9dbb-c84841e06f91"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1869), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("a05f6d6b-ccb2-43fd-8694-eceafdf0ab31"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1854), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("a3b279ea-2a65-4996-b0b5-ec242c90ebb2"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1849), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("d5d120a1-dba7-4e87-a4e1-9670a365dd2d"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1867), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "CurrencyRates",
                keyColumn: "Id",
                keyValue: new Guid("e4021ee6-1568-448c-a95b-ac76191f235b"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2023, 10, 5, 17, 35, 50, 406, DateTimeKind.Unspecified).AddTicks(1872), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
