using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Shortcuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Tasksets_CourseId_Name_Year",
                table: "Tasksets");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TasksetId",
                table: "Tasks");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Courses_Name",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "ShortcutCode",
                table: "Tasksets",
                type: "VARCHAR(8)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<short>(
                name: "InTasksetNumber",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortcutCode",
                table: "Courses",
                type: "VARCHAR(8)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasksets_CourseId_ShortcutCode",
                table: "Tasksets",
                columns: new[] { "CourseId", "ShortcutCode" },
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TasksetId_InTasksetNumber",
                table: "Tasks",
                columns: new[] { "TasksetId", "InTasksetNumber" },
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ShortcutCode",
                table: "Courses",
                column: "ShortcutCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasksets_CourseId_ShortcutCode",
                table: "Tasksets");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TasksetId_InTasksetNumber",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ShortcutCode",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ShortcutCode",
                table: "Tasksets");

            migrationBuilder.DropColumn(
                name: "InTasksetNumber",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ShortcutCode",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Tasksets_CourseId_Name_Year",
                table: "Tasksets",
                columns: new[] { "CourseId", "Name", "Year" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Courses_Name",
                table: "Courses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TasksetId",
                table: "Tasks",
                column: "TasksetId");
        }
    }
}
