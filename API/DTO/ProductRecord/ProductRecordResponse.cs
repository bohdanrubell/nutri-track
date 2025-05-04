namespace NutriTrack.DTO.ProductRecord;

public class ProductRecordResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Grams { get; set; }
    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public decimal Carbohydrates { get; set; }
}