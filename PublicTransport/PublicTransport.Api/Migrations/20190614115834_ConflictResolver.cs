using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class ConflictResolver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "UserDiscounts",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "TimeTables",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Tickets",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Stations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "StationLines",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Pricelists",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "PricelistItems",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Locations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Lines",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Items",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Busses",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TableVersion",
                table: "Adresses",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "UserDiscounts");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "StationLines");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Pricelists");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "PricelistItems");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Busses");

            migrationBuilder.DropColumn(
                name: "TableVersion",
                table: "Adresses");
        }
    }
}
