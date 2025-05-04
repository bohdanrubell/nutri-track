namespace NutriTrack.Entities;

public class Record
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DailyCalories { get; set; }
    public decimal DailyProtein { get; set; }
    public decimal DailyFat { get; set; }
    public decimal DailyCarbohydrates { get; set; }

    public int DiaryId { get; set; }
    public Diary Diary { get; set; }

    public int ActivityLogId { get; set; }
    public ActivityLevelLog ActivityLog { get; set; }

    public int GoalLogId { get; set; }
    public GoalTypeLog GoalLog { get; set; }

    public List<ProductRecord> ProductRecords { get; set; }


    public static Record Create(DateTime date, int dailyCalories, decimal dailyProtein, decimal dailyFat,
        decimal dailyCarbohydrates, Diary diary, ActivityLevelLog activityLog, GoalTypeLog goalLog)
    {
        var record = new Record
        {
            Date = date,
            DailyCalories = dailyCalories,
            DailyProtein = Math.Round(dailyProtein, 1),
            DailyFat = Math.Round(dailyFat,1),
            DailyCarbohydrates = Math.Round(dailyCarbohydrates,1),
            Diary = diary,
            ActivityLog = activityLog,
            GoalLog = goalLog,
        };
        
        return record;
    }
}