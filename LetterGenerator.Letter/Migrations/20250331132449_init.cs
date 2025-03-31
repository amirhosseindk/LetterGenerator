using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetterGenerator.Letter.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Letters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<string>(type: "TEXT", nullable: false),
                    DateTimeLocal = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RecipientName = table.Column<string>(type: "TEXT", nullable: true),
                    RecipientPosition = table.Column<string>(type: "TEXT", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    SenderName = table.Column<string>(type: "TEXT", nullable: true),
                    SenderPosition = table.Column<string>(type: "TEXT", nullable: true),
                    HaveCopy = table.Column<bool>(type: "INTEGER", nullable: false),
                    Copy = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDateTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedDateTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDateTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LetterSyncStatuses",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncType = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.DropTable(
                name: "Letters");
        }
    }
}
