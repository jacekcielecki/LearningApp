using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class addUserProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProgressId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperiencePoints = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProgressId",
                table: "Users",
                column: "UserProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProgresses_UserProgressId",
                table: "Users",
                column: "UserProgressId",
                principalTable: "UserProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProgresses_UserProgressId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProgressId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserProgressId",
                table: "Users");
        }
    }
}
