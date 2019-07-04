using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class PayPalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayPalInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    PayerEmail = table.Column<string>(nullable: true),
                    PayerId = table.Column<string>(nullable: true),
                    PayerFirstName = table.Column<string>(nullable: true),
                    PayerLastName = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PayerAccountStatus = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Total = table.Column<string>(nullable: true),
                    TableVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPalInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayPalInfos");
        }
    }
}
