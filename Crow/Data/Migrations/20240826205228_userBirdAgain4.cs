using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class userBirdAgain4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBird",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserBird_UserId",
                table: "UserBird",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBird_AspNetUsers_UserId",
                table: "UserBird",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBird_AspNetUsers_UserId",
                table: "UserBird");

            migrationBuilder.DropIndex(
                name: "IX_UserBird_UserId",
                table: "UserBird");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserBird",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
