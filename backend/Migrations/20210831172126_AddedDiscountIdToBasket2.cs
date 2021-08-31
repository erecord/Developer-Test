using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreBackend.Migrations
{
    public partial class AddedDiscountIdToBasket2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Basket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_UserId",
                table: "Basket",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_User_UserId",
                table: "Basket",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_User_UserId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_UserId",
                table: "Basket");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
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
