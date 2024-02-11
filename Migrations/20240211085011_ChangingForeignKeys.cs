using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class ChangingForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ListItems_UserId",
                table: "ListItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Users_UserId",
                table: "ListItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Users_UserId",
                table: "ListItems");

            migrationBuilder.DropIndex(
                name: "IX_ListItems_UserId",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ListItems");
        }
    }
}
