namespace NutriTrack.Entities;

public class Record
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DailyCalories { get; set; }
    public double DailyProtein { get; set; }
    public double DailyFat { get; set; }
    public double DailyCarbohydrates { get; set; }

    public int DiaryId { get; set; }
    public Diary Diary { get; set; }

    public int ActivityLogId { get; set; }
    public ActivityLevelLog ActivityLog { get; set; }

    public int GoalLogId { get; set; }
    public GoalTypeLog GoalLog { get; set; }

    public List<ProductRecord> ProductRecords { get; set; }


    public static Record Create(TimeProvider timeProvider, int dailyCalories, double dailyProtein, double dailyFat,
        double dailyCarbohydrates, Diary diary, ActivityLevelLog activityLog, GoalTypeLog goalLog)
    {
        var record = new Record
        {
            Date = timeProvider.GetUtcNow().Date,
            DailyCalories = dailyCalories,
            DailyProtein = dailyProtein,
            DailyFat = dailyFat,
            DailyCarbohydrates = dailyCarbohydrates,
            Diary = diary,
            ActivityLog = activityLog,
            GoalLog = goalLog,
        };
        
        return record;
    }
}