using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionGroupsTableRenamedToDistribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_DistributionGroups_GroupId",
                table: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "DistributionGroups");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "DistributionGroupItems",
                newName: "DistributionId");

            migrationBuilder.CreateTable(
                name: "Distribution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distribution", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_Distribution_DistributionId",
                table: "DistributionGroupItems",
                column: "DistributionId",
                principalTable: "Distribution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionGroupItems_Distribution_DistributionId",
                table: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "Distribution");

            migrationBuilder.RenameColumn(
                name: "DistributionId",
                table: "DistributionGroupItems",
                newName: "GroupId");

            migrationBuilder.CreateTable(
                name: "DistributionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionGroups", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionGroupItems_DistributionGroups_GroupId",
                table: "DistributionGroupItems",
                column: "GroupId",
                principalTable: "DistributionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
