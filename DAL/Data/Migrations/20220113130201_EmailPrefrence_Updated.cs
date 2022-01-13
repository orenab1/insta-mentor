using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class EmailPrefrence_Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_EmailPrefrenceAppUserId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EmailPrefrenceAppUserId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "WantToRecieveEmails",
                table: "EmailPrefrences",
                newName: "QuestionAskedOnMyTags");

            migrationBuilder.AddColumn<bool>(
                name: "MyQuestionReceivedNewComment",
                table: "EmailPrefrences",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MyQuestionReceivedNewOffer",
                table: "EmailPrefrences",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnlyNotifyOnCommunityQuestionAsked",
                table: "EmailPrefrences",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyQuestionReceivedNewComment",
                table: "EmailPrefrences");

            migrationBuilder.DropColumn(
                name: "MyQuestionReceivedNewOffer",
                table: "EmailPrefrences");

            migrationBuilder.DropColumn(
                name: "OnlyNotifyOnCommunityQuestionAsked",
                table: "EmailPrefrences");

            migrationBuilder.RenameColumn(
                name: "QuestionAskedOnMyTags",
                table: "EmailPrefrences",
                newName: "WantToRecieveEmails");

            migrationBuilder.AddColumn<int>(
                name: "EmailPrefrenceAppUserId",
                table: "Tags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_EmailPrefrenceAppUserId",
                table: "Tags",
                column: "EmailPrefrenceAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_EmailPrefrences_EmailPrefrenceAppUserId",
                table: "Tags",
                column: "EmailPrefrenceAppUserId",
                principalTable: "EmailPrefrences",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
