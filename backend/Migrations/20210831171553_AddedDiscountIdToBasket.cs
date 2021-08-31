using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreBackend.Migrations
{
    public partial class AddedDiscountIdToBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Basket",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Basket");
        }
    }
}
