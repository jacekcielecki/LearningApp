using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    /// <inheritdoc />
    public partial class unifyNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizzesToFinish",
                table: "LevelProgresses",
                newName: "QuizToFinish");

            migrationBuilder.RenameColumn(
                name: "FinishedQuizzes",
                table: "LevelProgresses",
                newName: "FinishedQuiz");

            migrationBuilder.RenameColumn(
                name: "QuestionsPerLesson",
                table: "Categories",
                newName: "QuizPerLevel");

            migrationBuilder.RenameColumn(
                name: "LessonsPerLevel",
                table: "Categories",
                newName: "QuestionsPerQuiz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizToFinish",
                table: "LevelProgresses",
                newName: "QuizzesToFinish");

            migrationBuilder.RenameColumn(
                name: "FinishedQuiz",
                table: "LevelProgresses",
                newName: "FinishedQuizzes");

            migrationBuilder.RenameColumn(
                name: "QuizPerLevel",
                table: "Categories",
                newName: "QuestionsPerLesson");

            migrationBuilder.RenameColumn(
                name: "QuestionsPerQuiz",
                table: "Categories",
                newName: "LessonsPerLevel");
        }
    }
}
