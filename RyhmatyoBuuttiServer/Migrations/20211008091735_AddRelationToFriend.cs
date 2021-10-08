using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RyhmatyoBuuttiServer.Migrations
{
    public partial class AddRelationToFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_User_UserId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_Friend_UserId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Friend");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Friend",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserEntityId",
                table: "Friend",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Entity_Id",
                table: "Friend",
                column: "UserEntityId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Entity_Id",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_Friend_UserEntityId",
                table: "Friend");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Friend",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Friend",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_User_UserId",
                table: "Friend",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
