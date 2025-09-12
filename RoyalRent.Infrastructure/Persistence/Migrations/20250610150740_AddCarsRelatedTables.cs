using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCarsRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarColors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarColors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CarMakes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    imageUrl = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMakes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CarTypes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    model = table.Column<string>(type: "VARCHAR(4)", nullable: false),
                    carMakeId = table.Column<Guid>(type: "UUID", nullable: false),
                    year = table.Column<int>(type: "INT", nullable: false),
                    carTypeId = table.Column<Guid>(type: "UUID", nullable: false),
                    carColorId = table.Column<Guid>(type: "UUID", nullable: false),
                    imageUrl = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_CAR_CAR_COLOR",
                        column: x => x.carColorId,
                        principalTable: "CarColors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAR_CAR_MAKE",
                        column: x => x.carMakeId,
                        principalTable: "CarMakes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAR_CAR_TYPE",
                        column: x => x.carTypeId,
                        principalTable: "CarTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarColors_name",
                table: "CarColors",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_CarMakes_name",
                table: "CarMakes",
                column: "name");

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

            migrationBuilder.CreateIndex(
                name: "IX_Cars_name_model_year_carMakeId_carColorId_carTypeId",
                table: "Cars",
                columns: new[] { "name", "model", "year", "carMakeId", "carColorId", "carTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CarTypes_name",
                table: "CarTypes",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarColors");

            migrationBuilder.DropTable(
                name: "CarMakes");

            migrationBuilder.DropTable(
                name: "CarTypes");
        }
    }
}
