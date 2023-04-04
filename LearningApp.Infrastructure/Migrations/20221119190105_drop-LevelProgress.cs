using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class dropLevelProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevelProgresses_CategoryProgresses_CategoryProgressId",
                table: "LevelProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LevelProgresses",
                table: "LevelProgresses");

            migrationBuilder.RenameTable(
                name: "LevelProgresses",
                newName: "LevelProgress");

            migrationBuilder.RenameIndex(
                name: "IX_LevelProgresses_CategoryProgressId",
                table: "LevelProgress",
                newName: "IX_LevelProgress_CategoryProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LevelProgress",
                table: "LevelProgress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LevelProgress_CategoryProgresses_CategoryProgressId",
                table: "LevelProgress",
                column: "CategoryProgressId",
                principalTable: "CategoryProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevelProgress_CategoryProgresses_CategoryProgressId",
                table: "LevelProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LevelProgress",
                table: "LevelProgress");

            migrationBuilder.RenameTable(
                name: "LevelProgress",
                newName: "LevelProgresses");

            migrationBuilder.RenameIndex(
                name: "IX_LevelProgress_CategoryProgressId",
                table: "LevelProgresses",
                newName: "IX_LevelProgresses_CategoryProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LevelProgresses",
                table: "LevelProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LevelProgresses_CategoryProgresses_CategoryProgressId",
                table: "LevelProgresses",
                column: "CategoryProgressId",
                principalTable: "CategoryProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
