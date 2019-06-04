using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicTransport.Api.Migrations
{
    public partial class ChangedPricelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PricelistItem_Items_ItemId",
                table: "PricelistItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PricelistItem_Pricelists_PricelistId",
                table: "PricelistItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_PricelistItem_PriceInfoId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricelistItem",
                table: "PricelistItem");

            migrationBuilder.RenameTable(
                name: "PricelistItem",
                newName: "PricelistItems");

            migrationBuilder.RenameIndex(
                name: "IX_PricelistItem_PricelistId",
                table: "PricelistItems",
                newName: "IX_PricelistItems_PricelistId");

            migrationBuilder.RenameIndex(
                name: "IX_PricelistItem_ItemId",
                table: "PricelistItems",
                newName: "IX_PricelistItems_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricelistItems",
                table: "PricelistItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscounts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PricelistItems_Items_ItemId",
                table: "PricelistItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PricelistItems_Pricelists_PricelistId",
                table: "PricelistItems",
                column: "PricelistId",
                principalTable: "Pricelists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_PricelistItems_PriceInfoId",
                table: "Tickets",
                column: "PriceInfoId",
                principalTable: "PricelistItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PricelistItems_Items_ItemId",
                table: "PricelistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PricelistItems_Pricelists_PricelistId",
                table: "PricelistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_PricelistItems_PriceInfoId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "UserDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricelistItems",
                table: "PricelistItems");

            migrationBuilder.RenameTable(
                name: "PricelistItems",
                newName: "PricelistItem");

            migrationBuilder.RenameIndex(
                name: "IX_PricelistItems_PricelistId",
                table: "PricelistItem",
                newName: "IX_PricelistItem_PricelistId");

            migrationBuilder.RenameIndex(
                name: "IX_PricelistItems_ItemId",
                table: "PricelistItem",
                newName: "IX_PricelistItem_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricelistItem",
                table: "PricelistItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PricelistItem_Items_ItemId",
                table: "PricelistItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PricelistItem_Pricelists_PricelistId",
                table: "PricelistItem",
                column: "PricelistId",
                principalTable: "Pricelists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_PricelistItem_PriceInfoId",
                table: "Tickets",
                column: "PriceInfoId",
                principalTable: "PricelistItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
