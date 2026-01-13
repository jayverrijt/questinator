using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questinator.Migrations
{
    /// <inheritdoc />
    public partial class AddLongDescriptionToStoreItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "StoreItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "StoreItems");
        }
    }
}
