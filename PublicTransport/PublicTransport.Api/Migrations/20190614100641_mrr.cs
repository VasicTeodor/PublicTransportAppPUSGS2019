using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class mrr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                table: "TimeTables",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                table: "TimeTables",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
