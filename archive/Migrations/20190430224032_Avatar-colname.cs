using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Avatarcolname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLogout",
                table: "AspNetUsers",
                newName: "LastActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastActive",
                table: "AspNetUsers",
                newName: "LastLogout");
        }
    }
}
