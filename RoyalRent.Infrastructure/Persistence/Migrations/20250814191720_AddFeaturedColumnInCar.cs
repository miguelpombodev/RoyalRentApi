using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturedColumnInCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFeatured",
                table: "Cars",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFeatured",
                table: "Cars");
        }
    }
}
