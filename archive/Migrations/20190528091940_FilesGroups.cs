using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class FilesGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileGroupEntries",
                columns: table => new
                {
                    FileGroupId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileGroupEntries", x => new { x.FileGroupId, x.FileId });
                    table.ForeignKey(
                        name: "FK_FileGroupEntries_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileGroupEntries_FileGroupId",
                table: "FileGroupEntries",
                column: "FileGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FileGroupEntries_FileId",
                table: "FileGroupEntries",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileGroupEntries");
        }
    }
}
