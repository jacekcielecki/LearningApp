using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAddCategoriesCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_AuthorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AuthorId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_CreatorId",
                table: "Categories",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_CreatorId",
                table: "Questions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_CreatorId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_CreatorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AuthorId",
                table: "Questions",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_AuthorId",
                table: "Questions",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
