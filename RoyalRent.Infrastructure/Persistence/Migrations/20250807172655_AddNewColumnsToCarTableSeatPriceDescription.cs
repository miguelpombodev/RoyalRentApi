using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnsToCarTableSeatPriceDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarFuelTypeId",
                table: "Cars",
                newName: "carFuelTypeId");

            migrationBuilder.RenameColumn(
                name: "CarTransmissionsId",
                table: "Cars",
                newName: "carTransmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarFuelTypeId",
                table: "Cars",
                newName: "IX_Cars_carFuelTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarTransmissionsId",
                table: "Cars",
                newName: "IX_Cars_carTransmissionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "carFuelTypeId",
                table: "Cars",
                type: "UUID",
                nullable: false,
                defaultValue: Guid.NewGuid(),
                oldClrType: typeof(Guid),
                oldType: "UUID",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "carTransmissionId",
                table: "Cars",
                type: "UUID",
                nullable: false,
                defaultValue: Guid.NewGuid(),
                oldClrType: typeof(Guid),
                oldType: "UUID",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Cars",
                type: "VARCHAR(3000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Cars",
                type: "numeric(16,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "seats",
                table: "Cars",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars",
                column: "carFuelTypeId",
                principalTable: "CarFuelTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars",
                column: "carTransmissionId",
                principalTable: "CarTransmissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_FUEL_TYPE",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CAR_CAR_TRANSMISSIONS",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "seats",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "carFuelTypeId",
                table: "Cars",
                newName: "CarFuelTypeId");

            migrationBuilder.RenameColumn(
                name: "carTransmissionId",
                table: "Cars",
                newName: "CarTransmissionsId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_carFuelTypeId",
                table: "Cars",
                newName: "IX_Cars_CarFuelTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_carTransmissionId",
                table: "Cars",
                newName: "IX_Cars_CarTransmissionsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarFuelTypeId",
                table: "Cars",
                type: "UUID",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UUID");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarTransmissionsId",
                table: "Cars",
                type: "UUID",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UUID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarFuelTypes_CarFuelTypeId",
                table: "Cars",
                column: "CarFuelTypeId",
                principalTable: "CarFuelTypes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTransmissions_CarTransmissionsId",
                table: "Cars",
                column: "CarTransmissionsId",
                principalTable: "CarTransmissions",
                principalColumn: "id");
        }
    }
}
