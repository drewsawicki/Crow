using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class BirdRemoveUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Bird");
        }

    }
}
