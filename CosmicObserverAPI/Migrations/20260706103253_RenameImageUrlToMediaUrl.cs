using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmicObserverAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameImageUrlToMediaUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "CosmicEvents",
                newName: "MediaUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "CosmicEvents",
                newName: "ImageUrl");
        }
    }
}
