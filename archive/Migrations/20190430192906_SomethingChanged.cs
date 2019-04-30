using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class SomethingChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_SolutionVersion_CurrentVersionId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionVersion_Solutions_SolutionId",
                table: "SolutionVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolutionVersion",
                table: "SolutionVersion");

            migrationBuilder.RenameTable(
                name: "SolutionVersion",
                newName: "SolutionsVersions");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionVersion_SolutionId",
                table: "SolutionsVersions",
                newName: "IX_SolutionsVersions_SolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolutionsVersions",
                table: "SolutionsVersions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_SolutionsVersions_CurrentVersionId",
                table: "Solutions",
                column: "CurrentVersionId",
                principalTable: "SolutionsVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionsVersions_Solutions_SolutionId",
                table: "SolutionsVersions",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_SolutionsVersions_CurrentVersionId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionsVersions_Solutions_SolutionId",
                table: "SolutionsVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolutionsVersions",
                table: "SolutionsVersions");

            migrationBuilder.RenameTable(
                name: "SolutionsVersions",
                newName: "SolutionVersion");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionsVersions_SolutionId",
                table: "SolutionVersion",
                newName: "IX_SolutionVersion_SolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolutionVersion",
                table: "SolutionVersion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_SolutionVersion_CurrentVersionId",
                table: "Solutions",
                column: "CurrentVersionId",
                principalTable: "SolutionVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionVersion_Solutions_SolutionId",
                table: "SolutionVersion",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
