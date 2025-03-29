namespace NutriTrack.DTO.Statistics;

public class PeriodStatisticsResponse
{
    public string Date { get; set; }
    public int ConsumedCalories { get; set; }
    public double ConsumedProteins { get; set; }
    public double ConsumedFats { get; set; }
    public double ConsumedCarbohydrates { get; set; }
    public string Status { get; set; }
}