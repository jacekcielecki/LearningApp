using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class addAchievements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    RequiredProgress = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AchievementUserProgress",
                columns: table => new
                {
                    AchievementsId = table.Column<int>(type: "int", nullable: false),
                    UserProgressesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementUserProgress", x => new { x.AchievementsId, x.UserProgressesId });
                    table.ForeignKey(
                        name: "FK_AchievementUserProgress_Achievements_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementUserProgress_UserProgresses_UserProgressesId",
                        column: x => x.UserProgressesId,
                        principalTable: "UserProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementUserProgress_UserProgressesId",
                table: "AchievementUserProgress",
                column: "UserProgressesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementUserProgress");

            migrationBuilder.DropTable(
                name: "Achievements");
        }
    }
}
