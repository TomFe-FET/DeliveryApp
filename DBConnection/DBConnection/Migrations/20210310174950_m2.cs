using Microsoft.EntityFrameworkCore.Migrations;

namespace DBConnection.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_OrderHead_DeliveryHeadID",
                table: "OrderLines");

            migrationBuilder.DropIndex(
                name: "IX_OrderLines_DeliveryHeadID",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "DeliveryHeadID",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "DeliveryHeadNo",
                table: "OrderLines");

            migrationBuilder.AddColumn<int>(
                name: "OrderHeadID",
                table: "OrderLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderHeadNo",
                table: "OrderLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderHeadID",
                table: "OrderLines",
                column: "OrderHeadID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_OrderHead_OrderHeadID",
                table: "OrderLines",
                column: "OrderHeadID",
                principalTable: "OrderHead",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_OrderHead_OrderHeadID",
                table: "OrderLines");

            migrationBuilder.DropIndex(
                name: "IX_OrderLines_OrderHeadID",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "OrderHeadID",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "OrderHeadNo",
                table: "OrderLines");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryHeadID",
                table: "OrderLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryHeadNo",
                table: "OrderLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_DeliveryHeadID",
                table: "OrderLines",
                column: "DeliveryHeadID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_OrderHead_DeliveryHeadID",
                table: "OrderLines",
                column: "DeliveryHeadID",
                principalTable: "OrderHead",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
