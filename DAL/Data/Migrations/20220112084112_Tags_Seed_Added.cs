using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Data.Migrations
{
    public partial class Tags_Seed_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "CreatorId", "EmailPrefrenceAppUserId", "IsApproved", "Text" },
                values: new object[] { 1, null, null, true, "SQL" });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "CreatorId", "EmailPrefrenceAppUserId", "IsApproved", "Text" },
                values: new object[] { 2, null, null, true, "Python" });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "CreatorId", "EmailPrefrenceAppUserId", "IsApproved", "Text" },
                values: new object[] { 3, null, null, true, "React" });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "CreatorId", "EmailPrefrenceAppUserId", "IsApproved", "Text" },
                values: new object[] { 4, null, null, true, "Angular" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
