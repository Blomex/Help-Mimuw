using Microsoft.EntityFrameworkCore.Migrations;

namespace archive.Migrations
{
    public partial class Tasksetsattachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TasksetId",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_TasksetId",
                table: "Files",
                column: "TasksetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Tasksets_TasksetId",
                table: "Files",
                column: "TasksetId",
                principalTable: "Tasksets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Tasksets_TasksetId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_TasksetId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "TasksetId",
                table: "Files");
        }
    }
}
