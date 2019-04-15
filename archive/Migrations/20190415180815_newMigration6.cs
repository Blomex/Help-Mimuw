using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class newMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "Ratings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
