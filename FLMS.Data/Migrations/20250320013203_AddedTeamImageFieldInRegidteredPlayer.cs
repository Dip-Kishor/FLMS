using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeamImageFieldInRegidteredPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "RegisteredPlayers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TeamImageUrl",
                table: "RegisteredPlayers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamImageUrl",
                table: "RegisteredPlayers");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "RegisteredPlayers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
