namespace NutriTrack.DTO.User;

public class UserCharacteristicsResponse
{
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public int Age { get; set; }
    public int Height { get; set; }
    public string CurrentGoalType { get; set; }
    public string CurrentActivityLevel { get; set; }
    public DailyNutritionsResponse DailyNutritions { get; set; }
    public List<WeightRecordResponse> WeightRecords { get; set; }
}