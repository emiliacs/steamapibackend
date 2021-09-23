using Microsoft.EntityFrameworkCore.Migrations;

namespace RyhmatyoBuuttiServer.Migrations
{
    public partial class ChangePublisherToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publishers",
                table: "Publisher",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Developer",
                newName: "Developers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Publisher",
                newName: "Publishers");

            migrationBuilder.RenameColumn(
                name: "Developers",
                table: "Developer",
                newName: "Name");
        }
    }
}
