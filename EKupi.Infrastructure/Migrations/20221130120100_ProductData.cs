using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKupi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("INSERT INTO [dbo].[Products] " +
               "([CategoryId]," +
               "[Name]," +
               "[UnitsInStock]," +
               "[UnitPrice]," +
               "[IsDeleted]) " +
               "VALUES" +
               "(1, 'Product 1', 50, 30.50, 0)," +
               "(1, 'Product 2', 60, 40.50, 0)," +
               "(2, 'Product 3', 40, 50.50, 0)," +
               "(3, 'Product 4', 30, 60.50, 0)," +
               "(1, 'Product 5', 20, 70.50, 0)," +
               "(4, 'Product 6', 10, 80.50, 0)," +
               "(5, 'Product 7', 80, 90.50, 0)," +
               "(5, 'Product 8', 100, 300.50, 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
