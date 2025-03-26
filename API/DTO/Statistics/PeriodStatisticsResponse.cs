namespace NutriTrack.DTO.Statistics;

public class PeriodStatisticsResponse
{
    public DateTime Date { get; set; }
    public double Calories { get; set; }
    public double Proteins { get; set; }
    public double Fats { get; set; }
    public double Carbohydrates { get; set; }
}