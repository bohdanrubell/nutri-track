namespace NutriTrack.Entities;

public class GoalType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Percent { get; set; }

    public List<GoalTypeLog> Logs { get; set; }
}