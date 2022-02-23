using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Question_addedPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Questions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_PhotoId",
                table: "Questions",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Photos_PhotoId",
                table: "Questions",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Photos_PhotoId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_PhotoId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Questions");
        }
    }
}
