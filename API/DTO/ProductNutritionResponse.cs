namespace NutriTrack.DTO;

public class ProductNutritionResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Calories { get; set; }
    public int Protein { get; set; }
    public int Fat { get; set; }
    public int Carbohydrates { get; set; }

    public string Category { get; set; }
}