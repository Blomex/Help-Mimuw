using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasksets_CourseId",
                table: "Tasksets");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Tasksets_CourseId_Name_Year",
                table: "Tasksets",
                columns: new[] { "CourseId", "Name", "Year" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Tasksets_CourseId_Name_Year",
                table: "Tasksets");

            migrationBuilder.CreateIndex(
                name: "IX_Tasksets_CourseId",
                table: "Tasksets",
                column: "CourseId");
        }
    }
}
