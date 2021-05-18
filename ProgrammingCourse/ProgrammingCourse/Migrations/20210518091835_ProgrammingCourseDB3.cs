using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingCourse.Migrations
{
    public partial class ProgrammingCourseDB3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTwoStepConfirmation",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OTPCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoStepConfirmation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OTPCode",
                table: "AspNetUsers");
        }
    }
}
