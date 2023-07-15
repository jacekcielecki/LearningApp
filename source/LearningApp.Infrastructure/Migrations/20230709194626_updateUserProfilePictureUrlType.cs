using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    /// <inheritdoc />
    public partial class updateUserProfilePictureUrlType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "varchar(2000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true);
        }
    }
}
