using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class connections_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_AppUserId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Connections",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_AppUserId",
                table: "Connections",
                newName: "IX_Connections_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_UserId",
                table: "Connections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_UserId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Connections",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_UserId",
                table: "Connections",
                newName: "IX_Connections_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_AppUserId",
                table: "Connections",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
