using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnIsSeasonCompleteInESeason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeasonComplete",
                table: "Seasons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeasonComplete",
                table: "Seasons");
        }
    }
}
