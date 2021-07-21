using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingCourse.Migrations
{
    public partial class ProgrammingCourseDB16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "CourseProcesses");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CourseProcesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CourseProcesses");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "CourseProcesses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
