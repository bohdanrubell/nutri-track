namespace NutriTrack.Entity;

public class WeightRecord
{
    public int Id { get; set; }
    public DateTime DateOfRecordCreated { get; set; }
    public int Weight { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
}