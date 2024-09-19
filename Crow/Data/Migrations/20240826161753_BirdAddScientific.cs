using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crow.Data.Migrations
{
    /// <inheritdoc />
    public partial class BirdAddScientific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Bird",
                newName: "ScientificName");

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Bird",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Bird");

            migrationBuilder.RenameColumn(
                name: "ScientificName",
                table: "Bird",
                newName: "Name");
        }
    }
}
