using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class DistributionSettingsTableShowSalesColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowSales",
                table: "DistributionSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowSales",
                table: "DistributionSettings");
        }
    }
}
