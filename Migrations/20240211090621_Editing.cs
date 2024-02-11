using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Editing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Users_UserId",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Lists_UserId",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Lists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lists_UserId",
                table: "Lists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_UserId",
                table: "Lists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
