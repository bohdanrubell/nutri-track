namespace NutriTrack.DTO.ProductNutrition;

public class CreateProductNutritionRequest
{
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public int CaloriesPer100Grams { get; set; }
    public int ProteinPer100Grams { get; set; }
    public int FatPer100Grams { get; set; }
    public int CarbohydratesPer100Grams { get; set; }
}