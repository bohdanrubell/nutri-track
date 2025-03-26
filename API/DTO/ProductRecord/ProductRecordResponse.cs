namespace NutriTrack.DTO.ProductRecord;

public class ProductRecordResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grams { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
}