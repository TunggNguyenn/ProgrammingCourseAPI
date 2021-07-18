using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingCourse.Migrations
{
    public partial class ProgrammingCourseDB15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseCarts");

            migrationBuilder.DropColumn(
                name: "Discription",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CourseId",
                table: "Carts",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Courses_CourseId",
                table: "Carts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Courses_CourseId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CourseId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CourseCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCarts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCarts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCarts_CartId",
                table: "CourseCarts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCarts_CourseId",
                table: "CourseCarts",
                column: "CourseId");
        }
    }
}
