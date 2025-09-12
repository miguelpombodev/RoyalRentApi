using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalRent.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovingCURRENTTIMESTAMPToUseAgnosticDatabaseDriven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "UsersDriverLicenses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "UsersDriverLicenses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "UsersAddresses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "UsersAddresses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "Users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "UsersDriverLicenses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "UsersDriverLicenses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "UsersAddresses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "UsersAddresses",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "Users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");
        }
    }
}
