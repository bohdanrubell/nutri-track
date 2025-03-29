namespace NutriTrack.DTO;

public class ProductNutritionResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
    public string Category { get; set; }
}