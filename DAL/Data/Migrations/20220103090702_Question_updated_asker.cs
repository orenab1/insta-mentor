using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Question_updated_asker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AskerId",
                table: "Questions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AskerId",
                table: "Questions",
                column: "AskerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_AskerId",
                table: "Questions",
                column: "AskerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_AskerId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AskerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AskerId",
                table: "Questions");
        }
    }
}
