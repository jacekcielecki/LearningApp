using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class updateCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Levels",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "QuestionsPerLevel",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "LessonsPerLevel",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsPerLesson",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonsPerLevel",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "QuestionsPerLesson",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Levels",
                table: "Categories",
                type: "int",
                maxLength: 400,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsPerLevel",
                table: "Categories",
                type: "int",
                maxLength: 400,
                nullable: false,
                defaultValue: 0);
        }
    }
}
