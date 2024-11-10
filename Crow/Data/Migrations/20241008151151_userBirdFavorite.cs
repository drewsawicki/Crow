using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class userBirdFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "UserBird",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "UserBird");
        }
    }
}
