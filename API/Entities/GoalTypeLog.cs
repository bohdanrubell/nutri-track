namespace NutriTrack.Entities;

public class GoalTypeLog
{
    public int Id { get; set; }
    public int GoalTypeId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public GoalType Goal { get; set; }
    public User User { get; set; }
}