using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudToDo.Migrations
{
    public partial class AddUserToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ActionItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_UserId",
                table: "ActionItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItems_Users_UserId",
                table: "ActionItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionItems_Users_UserId",
                table: "ActionItems");

            migrationBuilder.DropIndex(
                name: "IX_ActionItems_UserId",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActionItems");
        }
    }
}
