using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteBehaviorInCarsEntityConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_COLOR",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_MAKE",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_TYPE",
                table: "Cars");

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_COLOR",
                table: "Cars",
                column: "carColorId",
                principalTable: "CarColors",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars",
                column: "carFuelTypeId",
                principalTable: "CarFuelTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_MAKE",
                table: "Cars",
                column: "carMakeId",
                principalTable: "CarMakes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars",
                column: "carTransmissionId",
                principalTable: "CarTransmissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_TYPE",
                table: "Cars",
                column: "carTypeId",
                principalTable: "CarTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_COLOR",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_MAKE",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_TYPE",
                table: "Cars");

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_COLOR",
                table: "Cars",
                column: "carColorId",
                principalTable: "CarColors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars",
                column: "carFuelTypeId",
                principalTable: "CarFuelTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_MAKE",
                table: "Cars",
                column: "carMakeId",
                principalTable: "CarMakes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars",
                column: "carTransmissionId",
                principalTable: "CarTransmissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_TYPE",
                table: "Cars",
                column: "carTypeId",
                principalTable: "CarTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
