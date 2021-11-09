using Microsoft.EntityFrameworkCore.Migrations;

namespace motoShop.Migrations
{
    public partial class Orders4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Order",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Order",
                newName: "OrderId");
        }
    }
}
