using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTypesOfCPFC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ProteinPer100Grams",
                table: "ProductNutritions",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "FatPer100Grams",
                table: "ProductNutritions",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CarbohydratesPer100Grams",
                table: "ProductNutritions",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 14.0, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 23.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 12.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 8.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 18.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 10.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 11.0, 0.0, 4.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4.0, 0.0, 3.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4.0, 0.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 5.0, 1.0, 3.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 7.0, 2.0, 3.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 1.0, 33.0, 25.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 81.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 3.0, 4.0, 11.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 4.0, 31.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 15.0, 26.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 14.0, 27.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 7.0, 29.0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0.0, 21.0, 25.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProteinPer100Grams",
                table: "ProductNutritions",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "FatPer100Grams",
                table: "ProductNutritions",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "CarbohydratesPer100Grams",
                table: "ProductNutritions",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 14, 0, 0 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 23, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 12, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 8, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 18, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 10, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 11, 0, 4 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4, 0, 3 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 4, 0, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 5, 1, 3 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 7, 2, 3 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 1, 33, 25 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 81, 1 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 3, 4, 11 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 4, 31 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 15, 26 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 14, 27 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 7, 29 });

            migrationBuilder.UpdateData(
                table: "ProductNutritions",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CarbohydratesPer100Grams", "FatPer100Grams", "ProteinPer100Grams" },
                values: new object[] { 0, 21, 25 });
        }
    }
}
