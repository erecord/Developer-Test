using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreBackend.Migrations
{
    public partial class AddedDiscountIdToBasket1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Basket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_DiscountId",
                table: "Basket",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Discount_DiscountId",
                table: "Basket",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Discount_DiscountId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_DiscountId",
                table: "Basket");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Basket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
