using NutriTrack.DTO.ProductRecord;

namespace NutriTrack.DTO;

public class DairyRecordResponse
{
    public DailyNutritionsResponse DailyNutritions { get; set; }
    public List<ProductRecordResponse> ProductRecords { get; set; }
}