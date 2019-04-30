using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class RemovedCircularDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CurrentVersionId",
                table: "Solutions",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CurrentVersionId",
                table: "Solutions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
