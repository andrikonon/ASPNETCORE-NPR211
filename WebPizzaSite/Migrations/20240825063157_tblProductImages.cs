using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebPizzaSite.Migrations
{
    /// <inheritdoc />
    public partial class tblProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategory_CategoryId",
                table: "tblProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblProducts",
                table: "tblProducts");

            migrationBuilder.RenameTable(
                name: "tblProducts",
                newName: "tblProduct");

            migrationBuilder.RenameIndex(
                name: "IX_tblProducts_CategoryId",
                table: "tblProduct",
                newName: "IX_tblProduct_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblProduct",
                table: "tblProduct",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tblProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProductImage_tblProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProductImage_ProductId",
                table: "tblProductImage",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProduct_tblCategory_CategoryId",
                table: "tblProduct",
                column: "CategoryId",
                principalTable: "tblCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProduct_tblCategory_CategoryId",
                table: "tblProduct");

            migrationBuilder.DropTable(
                name: "tblProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblProduct",
                table: "tblProduct");

            migrationBuilder.RenameTable(
                name: "tblProduct",
                newName: "tblProducts");

            migrationBuilder.RenameIndex(
                name: "IX_tblProduct_CategoryId",
                table: "tblProducts",
                newName: "IX_tblProducts_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblProducts",
                table: "tblProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategory_CategoryId",
                table: "tblProducts",
                column: "CategoryId",
                principalTable: "tblCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
