using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopFilip.Migrations
{
    public partial class AddOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductAtributes_ProductsData_ProductId",
            //    table: "ProductAtributes");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductsData_Orders_OrderID",
            //    table: "ProductsData");

            //migrationBuilder.DropTable(
            //    name: "Orders");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProductsData_OrderID",
            //    table: "ProductsData");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ProductAtributes",
            //    table: "ProductAtributes");

            //migrationBuilder.DropColumn(
            //    name: "OrderID",
            //    table: "ProductsData");

            //migrationBuilder.RenameTable(
            //    name: "ProductAtributes",
            //    newName: "ProductAtribute");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ProductAtributes_ProductId",
            //    table: "ProductAtribute",
            //    newName: "IX_ProductAtribute_ProductId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ProductAtribute",
            //    table: "ProductAtribute",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductAtribute_ProductsData_ProductId",
            //    table: "ProductAtribute",
            //    column: "ProductId",
            //    principalTable: "ProductsData",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAtribute_ProductsData_ProductId",
                table: "ProductAtribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAtribute",
                table: "ProductAtribute");

            migrationBuilder.RenameTable(
                name: "ProductAtribute",
                newName: "ProductAtributes");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAtribute_ProductId",
                table: "ProductAtributes",
                newName: "IX_ProductAtributes_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "ProductsData",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAtributes",
                table: "ProductAtributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    DateOfOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsData_OrderID",
                table: "ProductsData",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAtributes_ProductsData_ProductId",
                table: "ProductAtributes",
                column: "ProductId",
                principalTable: "ProductsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsData_Orders_OrderID",
                table: "ProductsData",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
