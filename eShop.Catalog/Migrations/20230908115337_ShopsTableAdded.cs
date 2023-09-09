using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class ShopsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"), new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4879), new TimeSpan(0, 0, 0, 0, 0)), "Puma" },
                    { new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"), new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4862), new TimeSpan(0, 0, 0, 0, 0)), "Nike" },
                    { new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"), new DateTimeOffset(new DateTime(2023, 9, 8, 11, 53, 37, 147, DateTimeKind.Unspecified).AddTicks(4881), new TimeSpan(0, 0, 0, 0, 0)), "Adidas" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shops");
        }
    }
}
