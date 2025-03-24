using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGroupAandBtoGroupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupA",
                table: "FixturesAndResults");

            migrationBuilder.DropColumn(
                name: "IsGroupB",
                table: "FixturesAndResults");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MatchTime",
                table: "FixturesAndResults",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MatchDate",
                table: "FixturesAndResults",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "FixturesAndResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "FixturesAndResults");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MatchTime",
                table: "FixturesAndResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MatchDate",
                table: "FixturesAndResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
