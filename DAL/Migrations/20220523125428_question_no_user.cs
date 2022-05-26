using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class question_no_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NeededSkills",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeededSkills",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Questions");
        }
    }
}
