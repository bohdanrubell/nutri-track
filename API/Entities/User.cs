using Microsoft.AspNetCore.Identity;
using NutriTrack.Entity;
using NutriTrack.Entity.Enums;

namespace NutriTrack.Entities;

public class User : IdentityUser<int>
{
    public Gender UserGender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Height { get; set; }

    public Diary Diary { get; set; }
    public List<WeightRecord> WeightRecords { get; set; }
    public List<ActivityLevelLog> ActivityLevelLogs { get; set; }
    public List<GoalTypeLog> GoalTypeLogs { get; set; }
}