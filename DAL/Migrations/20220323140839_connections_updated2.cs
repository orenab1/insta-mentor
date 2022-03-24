using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class connections_updated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Connected",
                table: "Connections");

            migrationBuilder.AddColumn<DateTime>(
                name: "DisconnectedTime",
                table: "Connections",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisconnectedTime",
                table: "Connections");

            migrationBuilder.AddColumn<bool>(
                name: "Connected",
                table: "Connections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
