namespace NutriTrack.DTO;

public class UserCharacteristicsResponse
{
    public string Sex { get; set; }
    public int Age { get; set; }
    public int Height { get; set; }
    public string CurrentGoalType { get; set; }
    public string CurrentActivityLevel { get; set; }
    public DailyNutritionsResponse DailyNutritions { get; set; }
    public List<WeightRecordResponse> WeightRecords { get; set; }
}