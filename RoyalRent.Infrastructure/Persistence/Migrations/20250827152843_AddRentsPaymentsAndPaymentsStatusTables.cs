using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRentsPaymentsAndPaymentsStatusTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentsStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(40)", nullable: false),
                    statusColor = table.Column<string>(type: "CHAR(7)", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    userId = table.Column<Guid>(type: "UUID", nullable: false),
                    carId = table.Column<Guid>(type: "UUID", nullable: false),
                    rentStartsAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    rentFinishesAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    isPaid = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                    paymentAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: true),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.id);
                    table.ForeignKey(
                        name: "FK_RENT_CAR_ID",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RENT_USER_ID",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    userId = table.Column<Guid>(type: "UUID", nullable: false),
                    paymentStatusId = table.Column<Guid>(type: "UUID", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_PAYMENT_PAYMENT_STATUS",
                        column: x => x.paymentStatusId,
                        principalTable: "PaymentsStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_paymentStatusId",
                table: "Payments",
                column: "paymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_carId",
                table: "Rents",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_userId",
                table: "Rents",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "PaymentsStatus");
        }
    }
}
