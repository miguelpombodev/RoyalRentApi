using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDriverLicenseNumberLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CNH",
                table: "UsersDriverLicenses",
                type: "CHAR(11)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(9)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CNH",
                table: "UsersDriverLicenses",
                type: "CHAR(9)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(11)");
        }
    }
}
