using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFixtureAndResultsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGroupA",
                table: "FixturesAndResults",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupB",
                table: "FixturesAndResults",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupA",
                table: "FixturesAndResults");

            migrationBuilder.DropColumn(
                name: "IsGroupB",
                table: "FixturesAndResults");
        }
    }
}
