using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKupi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelatedProductsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRelationship_Products_ProductId",
                table: "ProductRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRelationship_Products_RelatedProductId",
                table: "ProductRelationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRelationship",
                table: "ProductRelationship");

            migrationBuilder.RenameTable(
                name: "ProductRelationship",
                newName: "ProductRelationships");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRelationship_RelatedProductId",
                table: "ProductRelationships",
                newName: "IX_ProductRelationships_RelatedProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRelationship_ProductId",
                table: "ProductRelationships",
                newName: "IX_ProductRelationships_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRelationships",
                table: "ProductRelationships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRelationships_Products_ProductId",
                table: "ProductRelationships",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRelationships_Products_RelatedProductId",
                table: "ProductRelationships",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRelationships_Products_ProductId",
                table: "ProductRelationships");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRelationships_Products_RelatedProductId",
                table: "ProductRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRelationships",
                table: "ProductRelationships");

            migrationBuilder.RenameTable(
                name: "ProductRelationships",
                newName: "ProductRelationship");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRelationships_RelatedProductId",
                table: "ProductRelationship",
                newName: "IX_ProductRelationship_RelatedProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRelationships_ProductId",
                table: "ProductRelationship",
                newName: "IX_ProductRelationship_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRelationship",
                table: "ProductRelationship",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRelationship_Products_ProductId",
                table: "ProductRelationship",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRelationship_Products_RelatedProductId",
                table: "ProductRelationship",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
