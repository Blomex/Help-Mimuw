using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace archive.Migrations
{
    public partial class SolutionEditingAndVersionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Solutions",
                newName: "CachedContent");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Solutions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CurrentVersionId",
                table: "Solutions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SolutionsVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SolutionId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionsVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolutionsVersions_Solutions_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            /* Take Solution's content and insert it as versions into SolutionVersion */
            migrationBuilder.Sql(@"
                INSERT INTO ""SolutionsVersions""(""SolutionId"", ""Created"", ""Content"")
                  SELECT ""Id"", NOW(), ""CachedContent"" FROM ""Solutions""
            ");
            /* Now set corresponging SolutionVersion.Id in table Solutions */
            /* WARNING: the syntax ist PostgreSQL-specific (see https://stackoverflow.com/questions/1293330/) */
            migrationBuilder.Sql(@"
                UPDATE ""Solutions""
                SET ""CurrentVersionId"" = ""SolutionsVersions"".""Id""
                FROM ""SolutionsVersions""
                  WHERE ""Solutions"".""Id"" = ""SolutionsVersions"".""SolutionId""
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_AuthorId",
                table: "Solutions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_CurrentVersionId",
                table: "Solutions",
                column: "CurrentVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionsVersions_SolutionId",
                table: "SolutionsVersions",
                column: "SolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_AspNetUsers_AuthorId",
                table: "Solutions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_SolutionsVersions_CurrentVersionId",
                table: "Solutions",
                column: "CurrentVersionId",
                principalTable: "SolutionsVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_AspNetUsers_AuthorId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_SolutionsVersions_CurrentVersionId",
                table: "Solutions");

            migrationBuilder.DropTable(
                name: "SolutionsVersions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_AuthorId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_CurrentVersionId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "CurrentVersionId",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "CachedContent",
                table: "Solutions",
                newName: "Content");
        }
    }
}
