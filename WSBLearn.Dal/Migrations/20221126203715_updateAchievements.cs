using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSBLearn.Dal.Migrations
{
    public partial class updateAchievements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "RequiredProgress",
                table: "Achievements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedOn",
                table: "Achievements",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Achievements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredProgress",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
