using NutriTrack.Entities.Enums;

namespace NutriTrack.DTO.Statistics;

public class PeriodStatisticsResponse
{
    public string Date { get; set; }
    public int ConsumedCalories { get; set; }
    public decimal ConsumedProteins { get; set; }
    public decimal ConsumedFats { get; set; }
    public decimal ConsumedCarbohydrates { get; set; }
    public string Status { get; set; }
    
    public List<string>? ExceededNutrients { get; set; }
}

public class NormStatusResult
{
    public NormStatus Status { get; set; }
    public List<string> ExceededNutrients { get; set; } = new();
}
