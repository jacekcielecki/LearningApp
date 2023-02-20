using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    /// <inheritdoc />
    public partial class updateCategorygroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoryGroups_CategoryGroupId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryGroupId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryGroupId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoryCategoryGroup",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    CategoryGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategoryGroup", x => new { x.CategoriesId, x.CategoryGroupsId });
                    table.ForeignKey(
                        name: "FK_CategoryCategoryGroup_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategoryGroup_CategoryGroups_CategoryGroupsId",
                        column: x => x.CategoryGroupsId,
                        principalTable: "CategoryGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategoryGroup_CategoryGroupsId",
                table: "CategoryCategoryGroup",
                column: "CategoryGroupsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategoryGroup");

            migrationBuilder.AddColumn<int>(
                name: "CategoryGroupId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryGroupId",
                table: "Categories",
                column: "CategoryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoryGroups_CategoryGroupId",
                table: "Categories",
                column: "CategoryGroupId",
                principalTable: "CategoryGroups",
                principalColumn: "Id");
        }
    }
}
