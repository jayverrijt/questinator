using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questinator.Migrations
{
    /// <inheritdoc />
    public partial class AddCoinsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Coins",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coins",
                table: "AspNetUsers");
        }
    }
}
