using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPasswordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersPasswords",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    password_hashed = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    user_id = table.Column<Guid>(type: "UUID", nullable: false),
                    actual_password = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    created_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    updated_on = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPasswords", x => x.id);
                    table.ForeignKey(
                        name: "FK_USER_USER_PASSWORD",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersPasswords_user_id_actual_password",
                table: "UsersPasswords",
                columns: new[] { "user_id", "actual_password" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersPasswords");
        }
    }
}
