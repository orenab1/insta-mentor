using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Tags_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Tag",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Tag",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatorId",
                table: "Tag",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Users_CreatorId",
                table: "Tag",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Users_CreatorId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_CreatorId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Tag");
        }
    }
}
