using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Viber.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViberUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSubcribed = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViberUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViberChatSettings",
                columns: table => new
                {
                    ViberUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViberChatSettings", x => x.ViberUserId);
                    table.ForeignKey(
                        name: "FK_ViberChatSettings_ViberUsers_ViberUserId",
                        column: x => x.ViberUserId,
                        principalTable: "ViberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViberUsers_ExternalId",
                table: "ViberUsers",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViberChatSettings");

            migrationBuilder.DropTable(
                name: "ViberUsers");
        }
    }
}
