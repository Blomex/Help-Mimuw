using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Tasksetsattachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TasksetsFiles",
                columns: table => new
                {
                    TasksetId = table.Column<int>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksetsFiles", x => new { x.TasksetId, x.FileId });
                    table.ForeignKey(
                        name: "FK_TasksetsFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksetsFiles_Tasksets_TasksetId",
                        column: x => x.TasksetId,
                        principalTable: "Tasksets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TasksetsFiles_FileId",
                table: "TasksetsFiles",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TasksetsFiles");
        }
    }
}
