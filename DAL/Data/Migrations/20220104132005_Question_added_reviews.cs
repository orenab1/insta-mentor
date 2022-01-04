using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Question_added_reviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Review_QuestionId",
                table: "Review",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Questions_QuestionId",
                table: "Review",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Questions_QuestionId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_QuestionId",
                table: "Review");
        }
    }
}
