using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Communities_updated_Desc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Communities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BestTimeToGetAnswer",
                table: "Communities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Communities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "BestTimeToGetAnswer",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Communities");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Communities",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
