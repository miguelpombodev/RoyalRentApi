using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCarTablesFuelTypeTransmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarFuelTypeId",
                table: "Cars",
                type: "UUID",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CarTransmissionsId",
                table: "Cars",
                type: "UUID",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarFuelTypes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFuelTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CarTransmissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTransmissions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarFuelTypeId",
                table: "Cars",
                column: "CarFuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarTransmissionsId",
                table: "Cars",
                column: "CarTransmissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CarFuelTypes_name",
                table: "CarFuelTypes",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_CarTransmissions_name",
                table: "CarTransmissions",
                column: "name");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarFuelTypes_CarFuelTypeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTransmissions_CarTransmissionsId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarFuelTypes");

            migrationBuilder.DropTable(
                name: "CarTransmissions");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarFuelTypeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarTransmissionsId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarFuelTypeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarTransmissionsId",
                table: "Cars");
        }
    }
}
