namespace NutriTrack.Entities;

public class ProductNutrition
{
    public int Id { get; set; }
    public int ProductNutritionCategoryId { get; set; }
    public string Name { get; set; }
    public int CaloriesPer100Grams { get; set; }
    public int ProteinPer100Grams { get; set; }
    public int FatPer100Grams { get; set; }
    public int CarbohydratesPer100Grams { get; set; }

    public ProductNutritionCategory ProductNutritionCategory { get; set; }
}