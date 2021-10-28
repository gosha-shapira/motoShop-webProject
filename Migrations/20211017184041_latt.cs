using Microsoft.EntityFrameworkCore.Migrations;

namespace motoShop.Migrations
{
    public partial class latt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Branches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Branches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Branches");
        }
    }
}
