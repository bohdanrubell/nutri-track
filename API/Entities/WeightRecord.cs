using NutriTrack.Entities;

namespace NutriTrack.Entity;

public class WeightRecord
{
    public int Id { get; set; }
    public DateTime DateOfRecordCreated { get; set; }
    public int Weight { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }

    public static WeightRecord Create(TimeProvider timeProvider, int weight, User user)
    {
        var weightRecord = new WeightRecord
        {
            DateOfRecordCreated = timeProvider.GetLocalNow().LocalDateTime,
            Weight = weight,
            User = user
        };
        
        return weightRecord;
    }
}