using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class lineToTimetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTables_Lines_LineId",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_LineId",
                table: "TimeTables");

            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                table: "TimeTables",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineId1",
                table: "TimeTables",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimetableId",
                table: "Lines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_LineId1",
                table: "TimeTables",
                column: "LineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTables_Lines_LineId1",
                table: "TimeTables",
                column: "LineId1",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTables_Lines_LineId1",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_LineId1",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "LineId1",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "TimetableId",
                table: "Lines");

            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                table: "TimeTables",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_LineId",
                table: "TimeTables",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTables_Lines_LineId",
                table: "TimeTables",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
