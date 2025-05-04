namespace NutriTrack.Entities;

public class GoalTypeLog
{
    public int Id { get; set; }
    public int GoalTypeId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public GoalType Goal { get; set; }
    public User User { get; set; }

    public static GoalTypeLog Create(TimeProvider timeProvider, GoalType goalType, User user)
    {
        var goalTypeLog = new GoalTypeLog
        {
            Date = timeProvider.GetLocalNow().LocalDateTime,
            Goal = goalType,
            User = user
        };

        return goalTypeLog;
    }
}