namespace NutriTrack.Entities;

public class ActivityLevel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Ratio { get; set; }
    
    public List<ActivityLevelLog> Logs { get; set; }
}