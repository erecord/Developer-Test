using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreBackend.Migrations
{
    public partial class AddedDiscountIdToBasket3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Discount_DiscountId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_User_UserId",
                table: "Basket");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Basket",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "Basket",
                newName: "discountId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_UserId",
                table: "Basket",
                newName: "IX_Basket_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_DiscountId",
                table: "Basket",
                newName: "IX_Basket_discountId");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Basket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "discountId",
                table: "Basket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Discount_discountId",
                table: "Basket",
                column: "discountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_User_userId",
                table: "Basket",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Discount_discountId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_User_userId",
                table: "Basket");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Basket",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "discountId",
                table: "Basket",
                newName: "DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_userId",
                table: "Basket",
                newName: "IX_Basket_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_discountId",
                table: "Basket",
                newName: "IX_Basket_DiscountId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Basket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Basket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Discount_DiscountId",
                table: "Basket",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_User_UserId",
                table: "Basket",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
