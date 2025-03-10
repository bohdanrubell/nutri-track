namespace NutriTrack.Entities;

public class ProductNutritionCategory
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ProductNutrition> ProductNutritions { get; set; }
}