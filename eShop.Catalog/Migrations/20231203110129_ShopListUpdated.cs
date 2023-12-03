using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class ShopListUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"));

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("08a8ac0c-3d28-4aff-8742-245dd9d644a3"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Mango" },
                    { new Guid("0f5c8ea0-9328-4dd6-88ad-893075a7faf2"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Macys" },
                    { new Guid("1f809d98-50cd-40d4-be1a-9835f4a81214"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Lefties" },
                    { new Guid("20c520b7-64fb-4b74-9574-7253df507af0"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Timberland" },
                    { new Guid("253d286d-e3bb-402f-b6ff-43b5a938ba17"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "JOMASHOP" },
                    { new Guid("2c466174-9364-4cff-88ec-4788209a5867"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Lidl" },
                    { new Guid("2de3d6cd-6e3e-42be-b0ec-032c0edefc21"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Zalando" },
                    { new Guid("3060dc9e-8caa-4cfd-b70a-aa299d22bfa6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Adidas" },
                    { new Guid("40837f56-0e75-4662-9038-2ad7577f23a8"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Under Armour" },
                    { new Guid("487adc27-6a38-41d6-a889-944e99071c1b"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Marc-o-Polo" },
                    { new Guid("510f26b9-a48c-407b-be73-ea604ea63379"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Clinique" },
                    { new Guid("52719937-2c2b-465d-b753-d5e7481e47a5"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "STRADIVARIUS" },
                    { new Guid("5b3fcdfb-7148-4ddf-b861-04b40c20bba3"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "DKNY" },
                    { new Guid("605fe585-29e4-4487-9e1f-f56b5f02a30d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "SMYK" },
                    { new Guid("68baf3ec-9bfd-4192-8ec1-d7cfe4c15805"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "GUESS Factory" },
                    { new Guid("697ec4ac-eace-4f86-b0c4-7804e45453a3"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "COACH" },
                    { new Guid("71f33e42-f7f9-463f-b7bb-9f4612547644"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Columbia" },
                    { new Guid("74c459df-4855-41e5-a064-5668f0546725"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Lesacoutlet" },
                    { new Guid("8105a063-b864-4818-a0a2-acb6d67b40c4"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "KARL LAGERFIELD" },
                    { new Guid("8e972f07-888e-4c70-9da9-367a4b6eded8"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bershka" },
                    { new Guid("966b6f67-b7d2-4f89-ab77-d3bc361cfeab"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Reebok UK" },
                    { new Guid("9c57533c-1324-4179-abc4-9b10e527a8e8"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Armani Exchange" },
                    { new Guid("9cd9e8a6-b2fa-4b90-a1f4-4fe9368fd31f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "MountainWarehouse" },
                    { new Guid("a1fe4c49-3db0-4e5b-a231-369530f6805d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "IHerb" },
                    { new Guid("a32d0b43-cfee-42cf-9ea9-b515e46d2ec3"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Columbia US" },
                    { new Guid("a3ae0d87-6acc-4fda-8bc2-08f6f14f803f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "NEXT" },
                    { new Guid("a9ad6042-33f8-46bc-84a0-fda1b92fd7cb"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Lacoste" },
                    { new Guid("af6cc85d-6610-49fb-9f65-18aa81303b4b"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Calvin Klein" },
                    { new Guid("af80fd3c-409e-4c53-b40e-5c07d9045275"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Tommy Hilfiger" },
                    { new Guid("b64d2e73-3f6b-4d51-82e4-f57e8a0afa63"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Desigual" },
                    { new Guid("b8f2f19f-4492-4090-bd4e-06be22abc51c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Oysho ES" },
                    { new Guid("bb836752-5e31-481f-a6c0-1e340d513224"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Puritan" },
                    { new Guid("be49c236-0ba1-4a8f-8f41-4adc996f25b5"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Nike US" },
                    { new Guid("c6c6e5fd-7d6c-4630-aee2-994a1bfffc4a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Zara PL" },
                    { new Guid("c8c878b0-8366-43f4-9cae-3c41e22b970d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Rituals США" },
                    { new Guid("cd13f11b-3e4b-49d3-ba73-646cee331db4"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Amazon UK, USA" },
                    { new Guid("cfa27462-df03-4a85-b7ea-872f12783d98"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Carters" },
                    { new Guid("cfd5b6a3-2582-4d2e-ba67-a2d0fc52ae2d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Gemini" },
                    { new Guid("dbcec2e7-c79e-4f3d-a9be-6547f060488a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Underarmour" },
                    { new Guid("e2c9af14-fdfc-4141-9dd7-e208e68372ba"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Women Secret" },
                    { new Guid("e924c4aa-c9c8-4984-b009-81d064b2ccb6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Zara" },
                    { new Guid("e92a6f82-f569-40e8-b50c-b2bb868de508"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "HM UK" },
                    { new Guid("ea7a3fb8-b981-45f1-a38d-021be1c5084e"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "GUESS" },
                    { new Guid("f5066b5c-794a-4ae0-b853-362f9bc1486a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PUMA " },
                    { new Guid("ffcaebf2-4807-4871-b547-6571c86c8597"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Mango Outlet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("08a8ac0c-3d28-4aff-8742-245dd9d644a3"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("0f5c8ea0-9328-4dd6-88ad-893075a7faf2"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("1f809d98-50cd-40d4-be1a-9835f4a81214"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("20c520b7-64fb-4b74-9574-7253df507af0"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("253d286d-e3bb-402f-b6ff-43b5a938ba17"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("2c466174-9364-4cff-88ec-4788209a5867"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("2de3d6cd-6e3e-42be-b0ec-032c0edefc21"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("3060dc9e-8caa-4cfd-b70a-aa299d22bfa6"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("40837f56-0e75-4662-9038-2ad7577f23a8"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("487adc27-6a38-41d6-a889-944e99071c1b"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("510f26b9-a48c-407b-be73-ea604ea63379"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("52719937-2c2b-465d-b753-d5e7481e47a5"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("5b3fcdfb-7148-4ddf-b861-04b40c20bba3"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("605fe585-29e4-4487-9e1f-f56b5f02a30d"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("68baf3ec-9bfd-4192-8ec1-d7cfe4c15805"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("697ec4ac-eace-4f86-b0c4-7804e45453a3"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("71f33e42-f7f9-463f-b7bb-9f4612547644"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("74c459df-4855-41e5-a064-5668f0546725"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("8105a063-b864-4818-a0a2-acb6d67b40c4"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("8e972f07-888e-4c70-9da9-367a4b6eded8"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("966b6f67-b7d2-4f89-ab77-d3bc361cfeab"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9c57533c-1324-4179-abc4-9b10e527a8e8"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("9cd9e8a6-b2fa-4b90-a1f4-4fe9368fd31f"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a1fe4c49-3db0-4e5b-a231-369530f6805d"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a32d0b43-cfee-42cf-9ea9-b515e46d2ec3"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a3ae0d87-6acc-4fda-8bc2-08f6f14f803f"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a9ad6042-33f8-46bc-84a0-fda1b92fd7cb"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("af6cc85d-6610-49fb-9f65-18aa81303b4b"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("af80fd3c-409e-4c53-b40e-5c07d9045275"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("b64d2e73-3f6b-4d51-82e4-f57e8a0afa63"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("b8f2f19f-4492-4090-bd4e-06be22abc51c"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("bb836752-5e31-481f-a6c0-1e340d513224"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("be49c236-0ba1-4a8f-8f41-4adc996f25b5"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("c6c6e5fd-7d6c-4630-aee2-994a1bfffc4a"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("c8c878b0-8366-43f4-9cae-3c41e22b970d"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("cd13f11b-3e4b-49d3-ba73-646cee331db4"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("cfa27462-df03-4a85-b7ea-872f12783d98"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("cfd5b6a3-2582-4d2e-ba67-a2d0fc52ae2d"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("dbcec2e7-c79e-4f3d-a9be-6547f060488a"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("e2c9af14-fdfc-4141-9dd7-e208e68372ba"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("e924c4aa-c9c8-4984-b009-81d064b2ccb6"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("e92a6f82-f569-40e8-b50c-b2bb868de508"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("ea7a3fb8-b981-45f1-a38d-021be1c5084e"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("f5066b5c-794a-4ae0-b853-362f9bc1486a"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("ffcaebf2-4807-4871-b547-6571c86c8597"));

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("17a27ec2-eb71-4de8-be81-734aeffd28f9"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Puma" },
                    { new Guid("7ebdc9b0-4846-415f-af63-29d74e4b7b36"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Nike" },
                    { new Guid("9afc47e2-3866-42f8-96db-1042800aafa7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Adidas" }
                });
        }
    }
}
