using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Contentcolumnincomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "content",
                table: "Comments",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "content");
        }
    }
}
