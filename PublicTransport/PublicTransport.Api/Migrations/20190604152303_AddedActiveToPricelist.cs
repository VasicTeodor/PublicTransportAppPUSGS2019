using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class AddedActiveToPricelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Pricelists",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Pricelists");
        }
    }
}
