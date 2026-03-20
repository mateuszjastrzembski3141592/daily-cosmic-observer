using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmicObserverAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CosmicEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    SourceUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmicEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CosmicTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmicTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CosmicLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CosmicEventId = table.Column<int>(type: "INTEGER", nullable: true),
                    SourceUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmicLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CosmicLogs_CosmicEvents_CosmicEventId",
                        column: x => x.CosmicEventId,
                        principalTable: "CosmicEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CosmicLogCosmicTag",
                columns: table => new
                {
                    LogsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmicLogCosmicTag", x => new { x.LogsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CosmicLogCosmicTag_CosmicLogs_LogsId",
                        column: x => x.LogsId,
                        principalTable: "CosmicLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CosmicLogCosmicTag_CosmicTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "CosmicTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CosmicLogCosmicTag_TagsId",
                table: "CosmicLogCosmicTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_CosmicLogs_CosmicEventId",
                table: "CosmicLogs",
                column: "CosmicEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CosmicLogCosmicTag");

            migrationBuilder.DropTable(
                name: "CosmicLogs");

            migrationBuilder.DropTable(
                name: "CosmicTags");

            migrationBuilder.DropTable(
                name: "CosmicEvents");
        }
    }
}
