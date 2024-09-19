using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class userBirdAgain5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBird_AspNetUsers_UserId",
                table: "UserBird");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBird",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_UserBird_UserId",
                table: "UserBird",
                newName: "IX_UserBird_User");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBird_AspNetUsers_User",
                table: "UserBird",
                column: "User",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBird_AspNetUsers_User",
                table: "UserBird");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "UserBird",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBird_User",
                table: "UserBird",
                newName: "IX_UserBird_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBird_AspNetUsers_UserId",
                table: "UserBird",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
