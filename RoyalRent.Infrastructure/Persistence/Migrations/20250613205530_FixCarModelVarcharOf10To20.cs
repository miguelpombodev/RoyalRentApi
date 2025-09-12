using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCarModelVarcharOf10To20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "model",
                table: "Cars",
                type: "VARCHAR(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "model",
                table: "Cars",
                type: "VARCHAR(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)");
        }
    }
}
