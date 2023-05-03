using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class dropLevelProgress2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelProgress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelProgress",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryProgressId = table.Column<int>(type: "int", nullable: false),
                    FinishedQuizzes = table.Column<int>(type: "int", nullable: false),
                    LevelCompleted = table.Column<bool>(type: "bit", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizzesToFinish = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelProgress_CategoryProgresses_CategoryProgressId",
                        column: x => x.CategoryProgressId,
                        principalTable: "CategoryProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelProgress_CategoryProgressId",
                table: "LevelProgress",
                column: "CategoryProgressId");
        }
    }
}
