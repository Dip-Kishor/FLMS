using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedIsCurrentSeasonInESeason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamImageUrl",
                table: "RegisteredPlayers");

            migrationBuilder.RenameColumn(
                name: "UserImageUrl",
                table: "RegisteredPlayers",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentSeason",
                table: "Seasons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrentSeason",
                table: "Seasons");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "RegisteredPlayers",
                newName: "UserImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "TeamImageUrl",
                table: "RegisteredPlayers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
