using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShop.Distribution.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "DistributionSettings",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSettings", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_DistributionSettings_Currencies_PreferredCurrencyId",
                        column: x => x.PreferredCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DistributionSettingsHistoryRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSettingsHistoryRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionSettingsHistoryRecord_Currencies_PreferredCurrencyId",
                        column: x => x.PreferredCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_DistributionSettings_DistributionSettingsAccountId",
                        column: x => x.DistributionSettingsAccountId,
                        principalTable: "DistributionSettings",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_Currencies_SourceCurrencyId",
                        column: x => x.SourceCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_Currencies_TargetCurrencyId",
                        column: x => x.TargetCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_DistributionSettings_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettings",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRateHistoryRecord",
                columns: table => new
                {
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    DistributionSettingsHistoryRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateHistoryRecord", x => new { x.DistributionSettingsId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistoryRecord_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecord_DistributionSettingsHistoryRecordId",
                        column: x => x.DistributionSettingsHistoryRecordId,
                        principalTable: "DistributionSettingsHistoryRecord",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsHistoryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelegramChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramChats_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViberChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViberChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViberChats_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistributionGroupItems",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TelegramChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ViberChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DistributionSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionGroupItems", x => new { x.GroupId, x.Id });
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_DistributionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DistributionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_DistributionSettingsHistoryRecord_DistributionSettingsId",
                        column: x => x.DistributionSettingsId,
                        principalTable: "DistributionSettingsHistoryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_TelegramChats_TelegramChatId",
                        column: x => x.TelegramChatId,
                        principalTable: "TelegramChats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionGroupItems_ViberChats_ViberChatId",
                        column: x => x.ViberChatId,
                        principalTable: "ViberChats",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"), "EUR" },
                    { new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"), "UAH" },
                    { new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"), "USD" }
                });

            migrationBuilder.InsertData(
                table: "CurrencyRates",
                columns: new[] { "Id", "CreatedAt", "DistributionSettingsId", "ModifiedAt", "Rate", "SourceCurrencyId", "TargetCurrencyId" },
                values: new object[,]
                {
                    { new Guid("69d06081-f197-4823-8248-eb6c60cb73a4"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4839), new TimeSpan(0, 0, 0, 0, 0)), null, null, 0.94999999999999996, new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"), new Guid("41ed0945-7196-4ead-8f5e-db262e62e536") },
                    { new Guid("6fc5e363-a92e-4412-9dbb-c84841e06f91"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4834), new TimeSpan(0, 0, 0, 0, 0)), null, null, 1.05, new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"), new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5") },
                    { new Guid("a05f6d6b-ccb2-43fd-8694-eceafdf0ab31"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4821), new TimeSpan(0, 0, 0, 0, 0)), null, null, 39.109999999999999, new Guid("41ed0945-7196-4ead-8f5e-db262e62e536"), new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34") },
                    { new Guid("a3b279ea-2a65-4996-b0b5-ec242c90ebb2"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4813), new TimeSpan(0, 0, 0, 0, 0)), null, null, 37.090000000000003, new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5"), new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34") },
                    { new Guid("d5d120a1-dba7-4e87-a4e1-9670a365dd2d"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4831), new TimeSpan(0, 0, 0, 0, 0)), null, null, 0.027, new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"), new Guid("bf879fb6-7b4b-41c7-9cc5-df8724d511e5") },
                    { new Guid("e4021ee6-1568-448c-a95b-ac76191f235b"), new DateTimeOffset(new DateTime(2023, 10, 2, 21, 26, 5, 917, DateTimeKind.Unspecified).AddTicks(4837), new TimeSpan(0, 0, 0, 0, 0)), null, null, 0.025999999999999999, new Guid("9724739e-e4b8-45eb-ac11-efe2b0558a34"), new Guid("41ed0945-7196-4ead-8f5e-db262e62e536") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DistributionSettingsAccountId",
                table: "Accounts",
                column: "DistributionSettingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateHistoryRecord_CurrencyId",
                table: "CurrencyRateHistoryRecord",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateHistoryRecord_DistributionSettingsHistoryRecordId",
                table: "CurrencyRateHistoryRecord",
                column: "DistributionSettingsHistoryRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_DistributionSettingsId_TargetCurrencyId_SourceCurrencyId",
                table: "CurrencyRates",
                columns: new[] { "DistributionSettingsId", "TargetCurrencyId", "SourceCurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_SourceCurrencyId",
                table: "CurrencyRates",
                column: "SourceCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_TargetCurrencyId",
                table: "CurrencyRates",
                column: "TargetCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_DistributionSettingsId",
                table: "DistributionGroupItems",
                column: "DistributionSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_TelegramChatId",
                table: "DistributionGroupItems",
                column: "TelegramChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionGroupItems_ViberChatId",
                table: "DistributionGroupItems",
                column: "ViberChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettings_PreferredCurrencyId",
                table: "DistributionSettings",
                column: "PreferredCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSettingsHistoryRecord_PreferredCurrencyId",
                table: "DistributionSettingsHistoryRecord",
                column: "PreferredCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChats_AccountId",
                table: "TelegramChats",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ViberChats_AccountId",
                table: "ViberChats",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRateHistoryRecord");

            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropTable(
                name: "DistributionGroupItems");

            migrationBuilder.DropTable(
                name: "DistributionGroups");

            migrationBuilder.DropTable(
                name: "DistributionSettingsHistoryRecord");

            migrationBuilder.DropTable(
                name: "TelegramChats");

            migrationBuilder.DropTable(
                name: "ViberChats");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "DistributionSettings");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
