namespace NutriTrack.DTO.User;

public class UpdateUserProfileRequest
{
    public string Gender { get; set; }
    public int Height { get; set; }
    public string CurrentGoalType { get; set; }
    public string CurrentActivityLevel { get; set; }
}