using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Tags_updated_TableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Users_CreatorId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_EmailPrefrenceAppUserId",
                table: "Tags",
                newName: "IX_Tags_EmailPrefrenceAppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_CreatorId",
                table: "Tags",
                newName: "IX_Tags_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tags",
                column: "EmailPrefrenceAppUserId",
                principalTable: "EmailPrefrences",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_CreatorId",
                table: "Tags",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_CreatorId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_EmailPrefrenceAppUserId",
                table: "Tag",
                newName: "IX_Tag_EmailPrefrenceAppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_CreatorId",
                table: "Tag",
                newName: "IX_Tag_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tag",
                column: "EmailPrefrenceAppUserId",
                principalTable: "EmailPrefrences",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Users_CreatorId",
                table: "Tag",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
