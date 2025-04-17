using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    name = table.Column<string>(type: "VARCHAR", maxLength: 650, nullable: false),
                    CPF = table.Column<string>(type: "CHAR(11)", nullable: false),
                    email = table.Column<string>(type: "VARCHAR", maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "CHAR(12)", nullable: false),
                    gender = table.Column<string>(type: "CHAR(1)", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAddresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    CEP = table.Column<string>(type: "CHAR(8)", nullable: true),
                    address = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    number = table.Column<string>(type: "VARCHAR", maxLength: 5, nullable: true),
                    neighborhood = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: true),
                    UF = table.Column<string>(type: "CHAR(2)", nullable: true),
                    user_id = table.Column<Guid>(type: "UUID", nullable: true),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAddresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_USER_USER_ADDRESSES",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UsersDriverLicenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    RG = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateOnly>(type: "DATE", nullable: true),
                    CNH = table.Column<string>(type: "CHAR(9)", nullable: true),
                    document_expiration_date = table.Column<DateOnly>(type: "DATE", nullable: true),
                    state = table.Column<string>(type: "CHAR(2)", nullable: true),
                    user_id = table.Column<Guid>(type: "UUID", nullable: true),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDriverLicenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_USER_USER_LICENSE",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_email_CPF_phone",
                table: "Users",
                columns: new[] { "email", "CPF", "phone" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersAddresses_CEP_address_city_id",
                table: "UsersAddresses",
                columns: new[] { "CEP", "address", "city", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersAddresses_user_id",
                table: "UsersAddresses",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersDriverLicenses_user_id",
                table: "UsersDriverLicenses",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersAddresses");

            migrationBuilder.DropTable(
                name: "UsersDriverLicenses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
