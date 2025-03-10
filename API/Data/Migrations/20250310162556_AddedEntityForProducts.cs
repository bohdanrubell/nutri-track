using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntityForProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductNutritionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNutritionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductNutritions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductNutritionCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CaloriesPer100Grams = table.Column<int>(type: "INTEGER", nullable: false),
                    ProteinPer100Grams = table.Column<int>(type: "INTEGER", nullable: false),
                    FatPer100Grams = table.Column<int>(type: "INTEGER", nullable: false),
                    CarbohydratesPer100Grams = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNutritions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductNutritions_ProductNutritionCategories_ProductNutritionCategoryId",
                        column: x => x.ProductNutritionCategoryId,
                        principalTable: "ProductNutritionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductNutritions_ProductNutritionCategoryId",
                table: "ProductNutritions",
                column: "ProductNutritionCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductNutritions");

            migrationBuilder.DropTable(
                name: "ProductNutritionCategories");
        }
    }
}
