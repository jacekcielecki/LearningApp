using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class AddLevelProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelProgresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinishedQuizzes = table.Column<int>(type: "int", nullable: false),
                    QuizzesToFinish = table.Column<int>(type: "int", nullable: false),
                    LevelCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CategoryProgressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelProgresses_CategoryProgresses_CategoryProgressId",
                        column: x => x.CategoryProgressId,
                        principalTable: "CategoryProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelProgresses_CategoryProgressId",
                table: "LevelProgresses",
                column: "CategoryProgressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelProgresses");
        }
    }
}
