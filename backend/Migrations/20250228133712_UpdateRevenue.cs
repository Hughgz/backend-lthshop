using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRevenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Revenues");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Revenues",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Revenues");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Revenues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Revenues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Revenues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
