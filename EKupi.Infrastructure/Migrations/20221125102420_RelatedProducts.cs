using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKupi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelatedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductRelationship",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RelatedProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRelationship_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRelationship_Products_RelatedProductId",
                        column: x => x.RelatedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationship_ProductId",
                table: "ProductRelationship",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationship_RelatedProductId",
                table: "ProductRelationship",
                column: "RelatedProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRelationship");
        }
    }
}
