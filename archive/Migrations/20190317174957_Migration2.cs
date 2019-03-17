using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Courses_Name",
                table: "Courses",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Courses_Name",
                table: "Courses");
        }
    }
}
