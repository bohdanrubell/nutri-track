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
    
    public static ActivityLevelLog Create(TimeProvider timeProvider, ActivityLevel activityLevel, User user)
    {
        var activityLevelLog = new ActivityLevelLog
        {
            Date = timeProvider.GetLocalNow().LocalDateTime,
            ActivityLevel = activityLevel,
            User = user
        };
        
        return activityLevelLog;
    }
}