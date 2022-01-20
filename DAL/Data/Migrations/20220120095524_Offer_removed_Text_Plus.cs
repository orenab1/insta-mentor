using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Offer_removed_Text_Plus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Questions_QuestionId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_OffererId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Offers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OffererId",
                table: "Offers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Questions_QuestionId",
                table: "Offers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_OffererId",
                table: "Offers",
                column: "OffererId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Questions_QuestionId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_OffererId",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Offers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OffererId",
                table: "Offers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Offers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Questions_QuestionId",
                table: "Offers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_OffererId",
                table: "Offers",
                column: "OffererId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
