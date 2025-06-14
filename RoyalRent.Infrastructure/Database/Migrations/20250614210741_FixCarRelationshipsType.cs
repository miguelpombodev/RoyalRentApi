using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixCarRelationshipsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_carColorId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_carMakeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_carTypeId",
                table: "Cars");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carColorId",
                table: "Cars",
                column: "carColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carMakeId",
                table: "Cars",
                column: "carMakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carTypeId",
                table: "Cars",
                column: "carTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_carColorId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_carMakeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_carTypeId",
                table: "Cars");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carColorId",
                table: "Cars",
                column: "carColorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carMakeId",
                table: "Cars",
                column: "carMakeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_carTypeId",
                table: "Cars",
                column: "carTypeId",
                unique: true);
        }
    }
}
