using NutriTrack.Entity;

namespace NutriTrack.Entities;

public class ActivityLevelLog
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public User User { get; set; }
}