using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class events_updated_topicIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TopicIdentifier",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopicIdentifier",
                table: "Events");
        }
    }
}
