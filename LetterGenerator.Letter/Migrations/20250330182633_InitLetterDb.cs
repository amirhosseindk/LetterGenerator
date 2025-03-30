using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetterGenerator.Letter.Migrations
{
    /// <inheritdoc />
    public partial class InitLetterDb : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Letters");
        }
    }
}
