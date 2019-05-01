using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace archive.Migrations
{
    public partial class Avatardeletedidcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars");

            migrationBuilder.DropIndex(
                name: "IX_Avatars_ApplicationUserId",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Avatars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Avatars",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_ApplicationUserId",
                table: "Avatars",
                column: "ApplicationUserId",
                unique: true);
        }
    }
}
