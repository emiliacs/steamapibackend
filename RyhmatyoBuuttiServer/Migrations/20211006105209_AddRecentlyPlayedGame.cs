using Microsoft.EntityFrameworkCore.Migrations;

namespace RyhmatyoBuuttiServer.Migrations
{
    public partial class AddRecentlyPlayedGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecentlyPlayedGame",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecentlyPlayedMinutes",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecentlyPlayedGame",
                table: "Friend",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecentlyPlayedMinutes",
                table: "Friend",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentlyPlayedGame",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RecentlyPlayedMinutes",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RecentlyPlayedGame",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "RecentlyPlayedMinutes",
                table: "Friend");
        }
    }
}
