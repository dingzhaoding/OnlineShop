using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopFilip.Migrations
{
    public partial class AddNewProductsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAtributes_Products_ProductId",
                table: "ProductAtributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductsData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsData",
                table: "ProductsData",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsData",
                table: "ProductsData");

            migrationBuilder.RenameTable(
                name: "ProductsData",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAtributes_Products_ProductId",
                table: "ProductAtributes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
