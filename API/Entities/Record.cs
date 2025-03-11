namespace NutriTrack.Entities;

public class Record
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public float DailyCalories { get; set; }
    public float DailyProtein { get; set; }
    public float DailyFat { get; set; }
    public float DailyCarbohydrates { get; set; }
    
    public int DiaryId { get; set; }
    public Diary Diary { get; set; }
    
    public int ActivityLogId { get; set; }
    public ActivityLevelLog ActivityLog { get; set; }
    
    public int GoalLogId { get; set; }
    public GoalTypeLog GoalLog { get; set; }

    public List<ProductRecord> ProductRecords { get; set; }
}