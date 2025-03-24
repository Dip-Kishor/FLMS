using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFixtureAndResultsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FixturesAndResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: false),
                    User1Score = table.Column<int>(type: "int", nullable: true),
                    User2Score = table.Column<int>(type: "int", nullable: true),
                    UserId2 = table.Column<int>(type: "int", nullable: false),
                    TiebrekerScoreUser1 = table.Column<int>(type: "int", nullable: true),
                    TiebrekerScoreUser2 = table.Column<int>(type: "int", nullable: true),
                    IsPostponed = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixturesAndResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixturesAndResults_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixturesAndResults_SeasonId",
                table: "FixturesAndResults",
                column: "SeasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixturesAndResults");
        }
    }
}
