using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class questionFeedbackRequestor_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionFeedbackRequestors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    RequestorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionFeedbackRequestors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionFeedbackRequestors_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionFeedbackRequestors_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFeedbackRequestors_QuestionId",
                table: "QuestionFeedbackRequestors",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFeedbackRequestors_RequestorId",
                table: "QuestionFeedbackRequestors",
                column: "RequestorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionFeedbackRequestors");
        }
    }
}
