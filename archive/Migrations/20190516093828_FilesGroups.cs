using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class FilesGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileGroupEntry",
                columns: table => new
                {
                    FileGroupId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileGroupEntry", x => new { x.FileGroupId, x.FileId });
                    table.ForeignKey(
                        name: "FK_FileGroupEntry_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileGroupEntry_FileGroupId",
                table: "FileGroupEntry",
                column: "FileGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FileGroupEntry_FileId",
                table: "FileGroupEntry",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileGroupEntry");
        }
    }
}
