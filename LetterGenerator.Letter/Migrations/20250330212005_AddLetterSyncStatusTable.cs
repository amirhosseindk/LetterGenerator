using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetterGenerator.Letter.Migrations
{
    /// <inheritdoc />
    public partial class AddLetterSyncStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LetterSyncStatuses",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastCheckedDateTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterSyncStatuses", x => new { x.LetterId, x.Username, x.DeviceType });
                    table.ForeignKey(
                        name: "FK_LetterSyncStatuses_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterSyncStatuses");
        }
    }
}
