using Microsoft.EntityFrameworkCore.Migrations;

namespace RyhmatyoBuuttiServer.Migrations
{
    public partial class AddImageToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Game",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Game");
       
        }
    }
}
