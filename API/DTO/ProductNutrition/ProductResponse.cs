namespace NutriTrack.DTO.ProductNutrition;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CaloriesPer100Grams { get; set; }
    public double ProteinPer100Grams { get; set; }
    public double FatPer100Grams { get; set; }
    public double CarbohydratesPer100Grams { get; set; }
    public string CategoryName { get; set; }

    public string ImageId { get; set; }
}