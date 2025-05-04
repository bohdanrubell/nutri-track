namespace NutriTrack.DTO.ProductNutrition;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public decimal CaloriesPer100Grams { get; set; }
    public decimal ProteinPer100Grams { get; set; }
    public decimal FatPer100Grams { get; set; }
    public decimal CarbohydratesPer100Grams { get; set; }

    public IFormFile? File { get; set; }
}