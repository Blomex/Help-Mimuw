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

            migrationBuilder.Sql("UPDATE \"Tasks\" SET \"CachedContent\" = \"Content\"");

            migrationBuilder.AlterColumn<string>(
                name: "CachedContent",
                table: "Tasks",
                nullable: false);


            migrationBuilder.AddColumn<string>(
                name: "CachedContent",
                table: "Comments",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"Comments\" SET \"CachedContent\" = \"Content\"");

            migrationBuilder.AlterColumn<string>(
                name: "CachedContent",
                table: "Comments",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CachedContent",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CachedContent",
                table: "Comments");
        }
    }
}
