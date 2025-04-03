namespace NutriTrack.Entities;

public class ProductNutrition
{
    public int Id { get; set; }
    public int ProductNutritionCategoryId { get; set; }
    public string Name { get; set; }
    public int CaloriesPer100Grams { get; set; }
    public double ProteinPer100Grams { get; set; }
    public double FatPer100Grams { get; set; }
    public double CarbohydratesPer100Grams { get; set; }

    public string? ImageUrl { get; set; }

    public ProductNutritionCategory ProductNutritionCategory { get; set; }
    
    public List<ProductRecord> ProductRecords { get; set; }
}