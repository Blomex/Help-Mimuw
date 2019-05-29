using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class MarkdownInCommentsAndTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CachedContent",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CachedContent",
                table: "Solutions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CachedContent",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CachedContent",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CachedContent",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "CachedContent",
                table: "Solutions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
