using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopFilip.Migrations
{
    public partial class ProductAtributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductAtributes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAtributes_ProductId",
                table: "ProductAtributes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAtributes_ProductsData_ProductId",
                table: "ProductAtributes",
                column: "ProductId",
                principalTable: "ProductsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAtributes_ProductsData_ProductId",
                table: "ProductAtributes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAtributes_ProductId",
                table: "ProductAtributes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductAtributes");
        }
    }
}
