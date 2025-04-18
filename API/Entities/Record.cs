﻿namespace NutriTrack.Entities;

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


    public static Record Create(DateTime date, int dailyCalories, double dailyProtein, double dailyFat,
        double dailyCarbohydrates, Diary diary, ActivityLevelLog activityLog, GoalTypeLog goalLog)
    {
        var record = new Record
        {
            Date = date,
            DailyCalories = dailyCalories,
            DailyProtein = Math.Round(dailyProtein, 2),
            DailyFat = Math.Round(dailyFat,2),
            DailyCarbohydrates = Math.Round(dailyCarbohydrates,2),
            Diary = diary,
            ActivityLog = activityLog,
            GoalLog = goalLog,
        };
        
        return record;
    }
}