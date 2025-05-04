namespace NutriTrack.Entities;

public class ProductNutrition
{
    public int Id { get; set; }
    public int ProductNutritionCategoryId { get; set; }
    public string Name { get; set; }
    public decimal CaloriesPer100Grams { get; set; }
    public decimal ProteinPer100Grams { get; set; }
    public decimal FatPer100Grams { get; set; }
    public decimal CarbohydratesPer100Grams { get; set; }

    public string? ImageUrl { get; set; }

    public ProductNutritionCategory ProductNutritionCategory { get; set; }

    public List<ProductRecord> ProductRecords { get; set; }
}