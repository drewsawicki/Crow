using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class userBirdAgain3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserBird_BirdId",
                table: "UserBird",
                column: "BirdId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBird_Bird_BirdId",
                table: "UserBird",
                column: "BirdId",
                principalTable: "Bird",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBird_Bird_BirdId",
                table: "UserBird");

            migrationBuilder.DropIndex(
                name: "IX_UserBird_BirdId",
                table: "UserBird");
        }
    }
}
