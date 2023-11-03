using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnounceProduct_Compositions_AnnouncesId",
                table: "AnnounceProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Compositions_AnnounceId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Shops_ShopId",
                table: "Compositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompositionImages",
                table: "CompositionImages");

            migrationBuilder.RenameTable(
                name: "Compositions",
                newName: "Announces");

            migrationBuilder.RenameTable(
                name: "CompositionImages",
                newName: "AnnounceImages");

            migrationBuilder.RenameIndex(
                name: "IX_Compositions_ShopId",
                table: "Announces",
                newName: "IX_Announces_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Compositions_OwnerId",
                table: "Announces",
                newName: "IX_Announces_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_AnnounceId",
                table: "AnnounceImages",
                newName: "IX_AnnounceImages_AnnounceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Announces",
                table: "Announces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnounceImages",
                table: "AnnounceImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnounceImages_Announces_AnnounceId",
                table: "AnnounceImages",
                column: "AnnounceId",
                principalTable: "Announces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnounceProduct_Announces_AnnouncesId",
                table: "AnnounceProduct",
                column: "AnnouncesId",
                principalTable: "Announces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Announces_Shops_ShopId",
                table: "Announces",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnounceImages_Announces_AnnounceId",
                table: "AnnounceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnounceProduct_Announces_AnnouncesId",
                table: "AnnounceProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Announces_Shops_ShopId",
                table: "Announces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Announces",
                table: "Announces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnounceImages",
                table: "AnnounceImages");

            migrationBuilder.RenameTable(
                name: "Announces",
                newName: "Compositions");

            migrationBuilder.RenameTable(
                name: "AnnounceImages",
                newName: "CompositionImages");

            migrationBuilder.RenameIndex(
                name: "IX_Announces_ShopId",
                table: "Compositions",
                newName: "IX_Compositions_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Announces_OwnerId",
                table: "Compositions",
                newName: "IX_Compositions_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AnnounceImages_AnnounceId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_AnnounceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompositionImages",
                table: "CompositionImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnounceProduct_Compositions_AnnouncesId",
                table: "AnnounceProduct",
                column: "AnnouncesId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Compositions_AnnounceId",
                table: "CompositionImages",
                column: "AnnounceId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Shops_ShopId",
                table: "Compositions",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
