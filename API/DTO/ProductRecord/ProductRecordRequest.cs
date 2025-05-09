namespace NutriTrack.DTO.ProductRecord;

public class ProductRecordRequest
{
    public int ProductNutritionId { get; set; }
    public decimal ConsumedGrams { get; set; }

    public DateTime? Date { get; set; }
}