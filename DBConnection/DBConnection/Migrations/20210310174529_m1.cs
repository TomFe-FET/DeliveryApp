using Microsoft.EntityFrameworkCore.Migrations;

namespace DBConnection.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderHead",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<string>(nullable: false),
                    DebNo = table.Column<int>(nullable: false),
                    DebName = table.Column<string>(nullable: false),
                    DebName2 = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHead", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinesID = table.Column<string>(nullable: false),
                    ArticleNo = table.Column<int>(nullable: false),
                    ArticleDescription = table.Column<string>(nullable: false),
                    ArticleDescription2 = table.Column<string>(nullable: true),
                    ArticleDescription3 = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    ReceiptNo = table.Column<int>(nullable: false),
                    DeliveryHeadID = table.Column<int>(nullable: false),
                    DeliveryHeadNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderLines_OrderHead_DeliveryHeadID",
                        column: x => x.DeliveryHeadID,
                        principalTable: "OrderHead",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_DeliveryHeadID",
                table: "OrderLines",
                column: "DeliveryHeadID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "OrderHead");
        }
    }
}
